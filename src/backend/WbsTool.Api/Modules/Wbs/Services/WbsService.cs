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
        var nodes = _dbContext.WbsNodes
            .AsNoTracking()
            .Include(w => w.Status)
            .Where(w => w.ProjectId == projectId && w.IsActive)
            .OrderBy(w => w.Level)
            .ThenBy(w => w.SortOrder)
            .ToList();

        return nodes.Select(MapToDto).ToList();
    }

    public IEnumerable<WbsTreeNodeDto> GetTreeByProjectId(Guid projectId)
    {
        var flatNodes = _dbContext.WbsNodes
            .AsNoTracking()
            .Include(w => w.Status)
            .Where(w => w.ProjectId == projectId && w.IsActive)
            .OrderBy(w => w.Level)
            .ThenBy(w => w.SortOrder)
            .ToList();

        var nodeLookup = flatNodes.ToDictionary(
            node => node.Id,
            node => MapToTreeDto(node));

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
                w.ProjectId == projectId &&
                w.IsActive);

            if (parent is null)
            {
                throw new ArgumentException("Parent node was not found in the specified project.");
            }

            calculatedLevel = parent.Level + 1;
        }

        var nextSortOrder = GetNextSortOrder(projectId, request.ParentId);
        var visibleWbsId = GenerateVisibleWbsId(projectId, request.ParentId, nextSortOrder);

        var duplicateVisibleWbsIdExists = _dbContext.WbsNodes.Any(w =>
            w.ProjectId == projectId &&
            w.IsActive &&
            w.VisibleWbsId == visibleWbsId);

        if (duplicateVisibleWbsIdExists)
        {
            throw new ArgumentException($"A WBS node with visible WBS ID '{visibleWbsId}' already exists in this project.");
        }

        var type = ParseNodeType(request.Type);

        var node = new WbsNode
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            ParentId = request.ParentId,
            VisibleWbsId = visibleWbsId,
            Title = request.Title,
            Description = request.Description,
            Type = type,
            Level = calculatedLevel,
            SortOrder = nextSortOrder,
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

        var createdNode = _dbContext.WbsNodes
            .AsNoTracking()
            .Include(w => w.Status)
            .First(w => w.Id == node.Id);

        return MapToDto(createdNode);
    }

    public WbsNodeDto? Update(Guid projectId, Guid id, UpdateWbsNodeRequest request)
    {
        var node = _dbContext.WbsNodes.FirstOrDefault(w =>
            w.Id == id &&
            w.ProjectId == projectId);

        if (node is null)
        {
            return null;
        }

        var duplicateVisibleWbsIdExists = _dbContext.WbsNodes.Any(w =>
            w.ProjectId == projectId &&
            w.IsActive &&
            w.Id != id &&
            w.VisibleWbsId == request.VisibleWbsId);

        if (duplicateVisibleWbsIdExists)
        {
            throw new ArgumentException($"A WBS node with visible WBS ID '{request.VisibleWbsId}' already exists in this project.");
        }

        var type = ParseNodeType(request.Type);

        node.VisibleWbsId = request.VisibleWbsId;
        node.Title = request.Title;
        node.Description = request.Description;
        node.Type = type;
        node.SortOrder = request.SortOrder;
        node.PlannedStart = request.PlannedStart;
        node.PlannedEnd = request.PlannedEnd;
        node.PlannedHours = request.PlannedHours;
        node.ActualHours = request.ActualHours;
        node.IsBlocked = request.IsBlocked;
        node.Comment = request.Comment;
        node.IsActive = request.IsActive;

        _dbContext.SaveChanges();

        var updatedNode = _dbContext.WbsNodes
            .AsNoTracking()
            .Include(w => w.Status)
            .First(w => w.Id == node.Id);

        return MapToDto(updatedNode);
    }

    public bool SoftDelete(Guid id)
    {
        var node = _dbContext.WbsNodes.FirstOrDefault(w => w.Id == id);

        if (node is null)
        {
            return false;
        }

        node.IsActive = false;
        _dbContext.SaveChanges();

        return true;
    }

    private int GetNextSortOrder(Guid projectId, Guid? parentId)
    {
        var siblingSortOrders = _dbContext.WbsNodes
            .Where(w =>
                w.ProjectId == projectId &&
                w.IsActive &&
                w.ParentId == parentId)
            .Select(w => (int?)w.SortOrder);

        var maxSortOrder = siblingSortOrders.Max();

        return (maxSortOrder ?? 0) + 1;
    }

    private string GenerateVisibleWbsId(Guid projectId, Guid? parentId, int sortOrder)
    {
        if (!parentId.HasValue)
        {
            return sortOrder.ToString();
        }

        var parent = _dbContext.WbsNodes.FirstOrDefault(w =>
            w.Id == parentId.Value &&
            w.ProjectId == projectId &&
            w.IsActive);

        if (parent is null)
        {
            throw new ArgumentException("Parent node was not found while generating visible WBS ID.");
        }

        return $"{parent.VisibleWbsId}.{sortOrder}";
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
            StatusId = node.StatusId,
            StatusLabel = node.Status?.Label,
            PlannedStart = node.PlannedStart,
            PlannedEnd = node.PlannedEnd,
            PlannedHoursTotal = node.PlannedHours ?? 0m,
            ActualHoursTotal = node.ActualHours ?? 0m,
            PlannedCostTotal = node.ImportedPlannedCost ?? 0m,
            ActualCostTotal = node.ImportedActualCost ?? 0m,
            IsBlocked = node.IsBlocked,
            Comment = node.Comment
        };
    }

    private static WbsTreeNodeDto MapToTreeDto(WbsNode node)
    {
        return new WbsTreeNodeDto
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
            StatusId = node.StatusId,
            StatusLabel = node.Status?.Label,
            PlannedStart = node.PlannedStart,
            PlannedEnd = node.PlannedEnd,
            PlannedHoursTotal = node.PlannedHours ?? 0m,
            ActualHoursTotal = node.ActualHours ?? 0m,
            PlannedCostTotal = node.ImportedPlannedCost ?? 0m,
            ActualCostTotal = node.ImportedActualCost ?? 0m,
            IsBlocked = node.IsBlocked,
            Comment = node.Comment,
            Children = new List<WbsTreeNodeDto>()
        };
    }
}