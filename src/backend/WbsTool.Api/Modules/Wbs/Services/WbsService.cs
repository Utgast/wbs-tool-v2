using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Wbs.Contracts;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Wbs.Services;

public class WbsService : IWbsService
{
    private readonly AppDbContext _dbContext;

    public WbsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Liefert alle WBS-Knoten eines Projekts als sortierte flache Liste.
    /// </summary>
    // Fachlicher Zweck: Die Sortierung nach Ebene und Reihenfolge schafft
    // eine stabile Ausgabe fuer Listenansichten und Folgeverarbeitung.
    public IEnumerable<WbsNodeDto> GetByProjectId(Guid projectId)
    {
        return _dbContext.WbsNodes
            .AsNoTracking()
            .Where(w => w.ProjectId == projectId)
            .OrderBy(w => w.Level)
            .ThenBy(w => w.SortOrder)
            .Select(MapToDto)
            .ToList();
    }

    /// <summary>
    /// Baut die WBS-Knoten eines Projekts zu einer Baumstruktur auf.
    /// </summary>
    // Fachlicher Zweck: Der Baum ist die zentrale Sicht fuer die fachliche
    // Strukturierung von Arbeitspaketen im Projekt.
    public IEnumerable<WbsTreeNodeDto> GetTreeByProjectId(Guid projectId)
    {
        var flatNodes = _dbContext.WbsNodes
            .AsNoTracking()
            .Where(w => w.ProjectId == projectId)
            .OrderBy(w => w.Level)
            .ThenBy(w => w.SortOrder)
            .ToList();

        // Wandelt die flache Ergebnismenge in ein schnelles Lookup um,
        // damit Parent/Child-Verknuepfungen ohne Mehrfachsuche aufgebaut werden koennen.
        var nodeLookup = flatNodes.ToDictionary(
            node => node.Id,
            node => new WbsTreeNodeDto
            {
                Id = node.Id,
                ProjectId = node.ProjectId,
                ParentId = node.ParentId,
                VisibleWbsId = node.VisibleWbsId,
                Code = node.Code,
                Title = node.Title,
                Description = node.Description,
                Status = node.Status.ToString(),
                ProgressPercent = node.ProgressPercent,
                ResponsiblePersonName = node.ResponsiblePersonName,
                Type = node.Type.ToString(),
                Level = node.Level,
                SortOrder = node.SortOrder,
                IsActive = node.IsActive,
                CreatedAt = node.CreatedAt,
                UpdatedAt = node.UpdatedAt,
                PlannedStart = node.PlannedStart,
                PlannedEnd = node.PlannedEnd,
                PlannedHours = node.PlannedHours,
                ActualHours = node.ActualHours,
                IsBlocked = node.IsBlocked,
                Comment = node.Comment,
                Children = new List<WbsTreeNodeDto>()
            });

        var rootNodes = new List<WbsTreeNodeDto>();

        // Baut den Baum fachlich korrekt auf: Kinder werden ihrem Parent zugeordnet,
        // Wurzelknoten ohne Parent bilden den Einstiegspunkt der WBS.
        foreach (var node in nodeLookup.Values.OrderBy(n => n.Level).ThenBy(n => n.SortOrder))
        {
            if (node.ParentId.HasValue && nodeLookup.TryGetValue(node.ParentId.Value, out var parentNode))
            {
                parentNode.Children.Add(node);
            }
            else
            {
                rootNodes.Add(node);
            }
        }

        return rootNodes;
    }

    /// <summary>
    /// Erstellt einen neuen WBS-Knoten innerhalb eines Projekts.
    /// </summary>
    // Fachlicher Zweck: Neue Arbeitspakete duerfen nur innerhalb eines gueltigen
    // Projekts und optional unter einem gueltigen Parent entstehen.
    public WbsNodeDto Create(Guid projectId, CreateWbsNodeRequest request)
    {
        var projectExists = _dbContext.Projects.Any(p => p.Id == projectId);

        if (!projectExists)
        {
            throw new ArgumentException($"Project with id '{projectId}' was not found.");
        }

        WbsNode? parent = null;
        var calculatedLevel = 1;

        if (request.ParentId.HasValue)
        {
            // Stellt sicher, dass Parent und Kind im selben Projekt liegen,
            // damit keine fachlich inkonsistenten Projekt-uebergreifenden Baeume entstehen.
            parent = _dbContext.WbsNodes.FirstOrDefault(w =>
                w.Id == request.ParentId.Value &&
                w.ProjectId == projectId);

            if (parent is null)
            {
                throw new ArgumentException("Parent node was not found in the specified project.");
            }

            calculatedLevel = parent.Level + 1;
        }

        // Konvertiert den fachlichen Knotentyp in das interne Enum,
        // damit nur bekannte WBS-Strukturelemente gespeichert werden.
        var type = ParseNodeType(request.Type);

        // Das Code-Feld wird additiv eingefuehrt und faellt fuer Bestandskompatibilitaet
        // auf VisibleWbsId zurueck, wenn der Client noch keinen separaten Code liefert.
        var code = string.IsNullOrWhiteSpace(request.Code)
            ? request.VisibleWbsId
            : request.Code.Trim();

        // Der Status wird im V1-Modell als eigener Fachzustand persistiert,
        // damit offene, blockierte und erledigte Arbeitspakete eindeutig auswertbar sind.
        var status = ParseNodeStatus(request.Status);

        // Fortschritt wird auf 0..100 normiert, damit Kennzahlen konsistent
        // fuer Dashboard und Projektsteuerung aggregiert werden koennen.
        var progressPercent = NormalizeProgressPercent(request.ProgressPercent);

        // Zeitstempel sichern die Nachvollziehbarkeit, wann Arbeitspakete erstellt
        // und zuletzt geaendert wurden.
        var nowUtc = DateTime.UtcNow;

        var node = new WbsNode
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            ParentId = request.ParentId,
            VisibleWbsId = request.VisibleWbsId,
            Code = code,
            Title = request.Title,
            Description = request.Description,
            Status = status,
            ProgressPercent = progressPercent,
            ResponsiblePersonName = request.ResponsiblePersonName?.Trim() ?? string.Empty,
            Type = type,
            Level = calculatedLevel,
            SortOrder = request.SortOrder,
            PlannedStart = request.PlannedStart,
            PlannedEnd = request.PlannedEnd,
            PlannedHours = request.PlannedHours,
            ActualHours = request.ActualHours,
            IsBlocked = request.IsBlocked,
            Comment = request.Comment,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc,
            IsActive = true
        };

        _dbContext.WbsNodes.Add(node);
        _dbContext.SaveChanges();

        return MapToDto(node);
    }

    private static WbsNodeType ParseNodeType(string type)
    {
        return type.Trim().ToLower() switch
        {
            "mainpackage" => WbsNodeType.MainPackage,
            "subpackage" => WbsNodeType.SubPackage,
            "task" => WbsNodeType.Task,
            _ => throw new ArgumentException($"Unsupported WBS node type '{type}'.")
        };
    }

    private static WbsNodeStatus ParseNodeStatus(string status)
    {
        var normalizedStatus = (status ?? string.Empty).Trim().ToLower();

        if (string.IsNullOrWhiteSpace(normalizedStatus))
        {
            return WbsNodeStatus.Offen;
        }

        return normalizedStatus switch
        {
            "offen" => WbsNodeStatus.Offen,
            "inbearbeitung" => WbsNodeStatus.InBearbeitung,
            "erledigt" => WbsNodeStatus.Erledigt,
            "blockiert" => WbsNodeStatus.Blockiert,
            _ => throw new ArgumentException($"Unsupported WBS node status '{status}'.")
        };
    }

    private static int NormalizeProgressPercent(int? progressPercent)
    {
        var value = progressPercent ?? 0;
        return Math.Clamp(value, 0, 100);
    }

    private static WbsNodeDto MapToDto(WbsNode node)
    {
        return new WbsNodeDto
        {
            Id = node.Id,
            ProjectId = node.ProjectId,
            ParentId = node.ParentId,
            VisibleWbsId = node.VisibleWbsId,
            Code = node.Code,
            Title = node.Title,
            Description = node.Description,
            Status = node.Status.ToString(),
            ProgressPercent = node.ProgressPercent,
            ResponsiblePersonName = node.ResponsiblePersonName,
            Type = node.Type.ToString(),
            Level = node.Level,
            SortOrder = node.SortOrder,
            IsActive = node.IsActive,
            CreatedAt = node.CreatedAt,
            UpdatedAt = node.UpdatedAt,
            PlannedStart = node.PlannedStart,
            PlannedEnd = node.PlannedEnd,
            PlannedHours = node.PlannedHours,
            ActualHours = node.ActualHours,
            IsBlocked = node.IsBlocked,
            Comment = node.Comment
        };
    }
}