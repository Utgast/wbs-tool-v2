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

    public IEnumerable<WbsTreeNodeDto> GetTreeByProjectId(Guid projectId)
    {
        var flatNodes = _dbContext.WbsNodes
            .AsNoTracking()
            .Where(w => w.ProjectId == projectId)
            .OrderBy(w => w.Level)
            .ThenBy(w => w.SortOrder)
            .ToList();

        var nodeLookup = flatNodes.ToDictionary(
            node => node.Id,
            node => new WbsTreeNodeDto
            {
                Id = node.Id,
                ProjectId = node.ProjectId,
                ParentId = node.ParentId,
                VisibleWbsId = node.VisibleWbsId,
                Title = node.Title,
                Description = node.Description,
                Type = node.Type.ToString(),
                Level = node.Level,
                SortOrder = node.SortOrder,
                IsActive = node.IsActive,
                PlannedStart = node.PlannedStart,
                PlannedEnd = node.PlannedEnd,
                PlannedHours = node.PlannedHours,
                ActualHours = node.ActualHours,
                IsBlocked = node.IsBlocked,
                Comment = node.Comment,
                Children = new List<WbsTreeNodeDto>()
            });

        var rootNodes = new List<WbsTreeNodeDto>();

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
            parent = _dbContext.WbsNodes.FirstOrDefault(w =>
                w.Id == request.ParentId.Value &&
                w.ProjectId == projectId);

            if (parent is null)
            {
                throw new ArgumentException("Parent node was not found in the specified project.");
            }

            calculatedLevel = parent.Level + 1;
        }

        var type = ParseNodeType(request.Type);

        var node = new WbsNode
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            ParentId = request.ParentId,
            VisibleWbsId = request.VisibleWbsId,
            Title = request.Title,
            Description = request.Description,
            Type = type,
            Level = calculatedLevel,
            SortOrder = request.SortOrder,
            PlannedStart = request.PlannedStart,
            PlannedEnd = request.PlannedEnd,
            PlannedHours = request.PlannedHours,
            ActualHours = request.ActualHours,
            IsBlocked = request.IsBlocked,
            Comment = request.Comment,
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

    private static WbsNodeDto MapToDto(WbsNode node)
    {
        return new WbsNodeDto
        {
            Id = node.Id,
            ProjectId = node.ProjectId,
            ParentId = node.ParentId,
            VisibleWbsId = node.VisibleWbsId,
            Title = node.Title,
            Description = node.Description,
            Type = node.Type.ToString(),
            Level = node.Level,
            SortOrder = node.SortOrder,
            IsActive = node.IsActive,
            PlannedStart = node.PlannedStart,
            PlannedEnd = node.PlannedEnd,
            PlannedHours = node.PlannedHours,
            ActualHours = node.ActualHours,
            IsBlocked = node.IsBlocked,
            Comment = node.Comment
        };
    }
}