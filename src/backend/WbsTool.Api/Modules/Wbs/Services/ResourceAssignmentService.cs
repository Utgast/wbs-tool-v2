using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Wbs.Contracts;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Wbs.Services;

/// <summary>
/// Resource Assignment Service - Verwaltung von Ressourcen-Zuordnungen
/// 
/// Verantwortung:
/// - Zuweisung von Personen zu WBS Knoten
/// - Verfolgung von Stunden pro Rolle/Person/Node
/// - Tarifkategorien für geplante und tatsächliche Kosten
/// 
/// WICHTIG: ResourceAssignments sind DETAIL-Daten!
/// - Mehrere Personen können einem Node zugeordnet werden
/// - Dashboard-Totale verwenden WbsNode.PlannedHours/ActualHours, NICHT die Summe der Assignments
/// - Das verhindert Doppelzählungen bei mehreren Personen pro Node
/// 
/// Architektur: Service mit direktem DbContext Zugriff (Data Access nicht separiert).
/// Validiert Abhängigkeiten: WbsNode, Person, RateCategories existieren.
/// </summary>
public class ResourceAssignmentService : IResourceAssignmentService
{
    private readonly AppDbContext _dbContext;

    public ResourceAssignmentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Alle aktiven Ressourcen-Zuordnungen für einen WBS Knoten abrufen
    /// </summary>
    public IEnumerable<ResourceAssignmentDto> GetByWbsNode(Guid projectId, Guid wbsNodeId)
    {
        return _dbContext.ResourceAssignments
            .AsNoTracking()
            .Include(x => x.WbsNode)
            .Include(x => x.Person)
            .Include(x => x.PlannedRateCategory)
            .Include(x => x.ActualRateCategory)
            .Where(x =>
                x.WbsNodeId == wbsNodeId &&
                x.WbsNode.ProjectId == projectId &&
                x.IsActive)
            .OrderBy(x => x.Person.DisplayName)
            .ThenBy(x => x.AssignmentRole)
            .Select(MapToDto())
            .ToList();
    }

    /// <summary>
    /// Neue Ressourcen-Zuordnung erstellen
    /// Validiert: WbsNode existiert, Person existiert, Tarifkategorien existieren
    /// </summary>
    public ResourceAssignmentDto Create(
        Guid projectId,
        Guid wbsNodeId,
        CreateResourceAssignmentRequest request)
    {
        EnsureWbsNodeExists(projectId, wbsNodeId);
        EnsurePersonExists(request.PersonId);
        EnsureRateCategoriesExist(request.PlannedRateCategoryId, request.ActualRateCategoryId);

        var now = DateTime.UtcNow;

        var entity = new ResourceAssignment
        {
            Id = Guid.NewGuid(),
            WbsNodeId = wbsNodeId,
            PersonId = request.PersonId,
            AssignmentRole = request.AssignmentRole.Trim(),
            PlannedRateCategoryId = request.PlannedRateCategoryId,
            ActualRateCategoryId = request.ActualRateCategoryId,
            PlannedHours = request.PlannedHours,
            ActualHours = request.ActualHours,
            Comment = request.Comment?.Trim() ?? string.Empty,
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        _dbContext.ResourceAssignments.Add(entity);
        _dbContext.SaveChanges();

        return LoadDto(entity.Id)
            ?? throw new ArgumentException("Created resource assignment could not be loaded.");
    }

    /// <summary>
    /// Ressourcen-Zuordnung aktualisieren (Stunden, Tarifkategorien, Rolle)
    /// </summary>
    public ResourceAssignmentDto? Update(
        Guid projectId,
        Guid wbsNodeId,
        Guid assignmentId,
        UpdateResourceAssignmentRequest request)
    {
        var entity = _dbContext.ResourceAssignments
            .Include(x => x.WbsNode)
            .FirstOrDefault(x =>
                x.Id == assignmentId &&
                x.WbsNodeId == wbsNodeId &&
                x.WbsNode.ProjectId == projectId);

        if (entity is null)
        {
            return null;
        }

        EnsurePersonExists(request.PersonId);
        EnsureRateCategoriesExist(request.PlannedRateCategoryId, request.ActualRateCategoryId);

        entity.PersonId = request.PersonId;
        entity.AssignmentRole = request.AssignmentRole.Trim();
        entity.PlannedRateCategoryId = request.PlannedRateCategoryId;
        entity.ActualRateCategoryId = request.ActualRateCategoryId;
        entity.PlannedHours = request.PlannedHours;
        entity.ActualHours = request.ActualHours;
        entity.Comment = request.Comment?.Trim() ?? string.Empty;
        entity.IsActive = request.IsActive;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        _dbContext.SaveChanges();

        return LoadDto(entity.Id);
    }

    /// <summary>
    /// Ressourcen-Zuordnung softlöschen (IsActive = false)
    /// Erhält historische Daten für Audit Trail
    /// </summary>
    public bool Deactivate(Guid projectId, Guid wbsNodeId, Guid assignmentId)
    {
        var entity = _dbContext.ResourceAssignments
            .Include(x => x.WbsNode)
            .FirstOrDefault(x =>
                x.Id == assignmentId &&
                x.WbsNodeId == wbsNodeId &&
                x.WbsNode.ProjectId == projectId);

        if (entity is null)
        {
            return false;
        }

        entity.IsActive = false;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        _dbContext.SaveChanges();
        return true;
    }

    private void EnsureWbsNodeExists(Guid projectId, Guid wbsNodeId)
    {
        var exists = _dbContext.WbsNodes.Any(x =>
            x.Id == wbsNodeId &&
            x.ProjectId == projectId &&
            x.IsActive);

        if (!exists)
        {
            throw new ArgumentException("WBS node not found in the given project.");
        }
    }

    private void EnsurePersonExists(Guid personId)
    {
        var exists = _dbContext.Persons.Any(x =>
            x.Id == personId &&
            x.IsActive);

        if (!exists)
        {
            throw new ArgumentException("Person not found.");
        }
    }

    private void EnsureRateCategoriesExist(Guid? plannedRateCategoryId, Guid? actualRateCategoryId)
    {
        if (plannedRateCategoryId.HasValue)
        {
            var plannedExists = _dbContext.RateCategories.Any(x =>
                x.Id == plannedRateCategoryId.Value &&
                x.IsActive);

            if (!plannedExists)
            {
                throw new ArgumentException("Planned rate category not found.");
            }
        }

        if (actualRateCategoryId.HasValue)
        {
            var actualExists = _dbContext.RateCategories.Any(x =>
                x.Id == actualRateCategoryId.Value &&
                x.IsActive);

            if (!actualExists)
            {
                throw new ArgumentException("Actual rate category not found.");
            }
        }
    }

    private ResourceAssignmentDto? LoadDto(Guid assignmentId)
    {
        return _dbContext.ResourceAssignments
            .AsNoTracking()
            .Include(x => x.Person)
            .Include(x => x.PlannedRateCategory)
            .Include(x => x.ActualRateCategory)
            .Where(x => x.Id == assignmentId)
            .Select(MapToDto())
            .FirstOrDefault();
    }

    private static System.Linq.Expressions.Expression<Func<ResourceAssignment, ResourceAssignmentDto>> MapToDto()
    {
        return x => new ResourceAssignmentDto
        {
            Id = x.Id,
            WbsNodeId = x.WbsNodeId,
            PersonId = x.PersonId,
            PersonDisplayName = x.Person.DisplayName,
            PersonIsPlaceholder = x.Person.IsPlaceholder,
            AssignmentRole = x.AssignmentRole,

            PlannedRateCategoryId = x.PlannedRateCategoryId,
            PlannedRateCategoryCode = x.PlannedRateCategory != null
                ? x.PlannedRateCategory.Code
                : null,

            ActualRateCategoryId = x.ActualRateCategoryId,
            ActualRateCategoryCode = x.ActualRateCategory != null
                ? x.ActualRateCategory.Code
                : null,

            PlannedHours = x.PlannedHours,
            ActualHours = x.ActualHours,

            PlannedCost =
                x.PlannedHours.HasValue && x.PlannedRateCategory != null
                    ? x.PlannedHours.Value * x.PlannedRateCategory.HourlyRate
                    : null,

            ActualCost =
                x.ActualHours.HasValue && x.ActualRateCategory != null
                    ? x.ActualHours.Value * x.ActualRateCategory.HourlyRate
                    : null,

            Comment = x.Comment,
            IsActive = x.IsActive
        };
    }
}