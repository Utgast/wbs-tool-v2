using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.ResourceDemands.Contracts;
using WbsTool.Api.Modules.ResourceDemands.Models;

namespace WbsTool.Api.Modules.ResourceDemands.Services;

public class ResourceDemandService : IResourceDemandService
{
    private readonly AppDbContext _dbContext;

    public ResourceDemandService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<ResourceDemandDto> GetAll()
    {
        return _dbContext.ResourceDemands
            .AsNoTracking()
            .OrderBy(r => r.CreatedAtUtc)
            .Select(MapToDto)
            .ToList();
    }

    public ResourceDemandDto? GetById(Guid id)
    {
        var demand = _dbContext.ResourceDemands
            .AsNoTracking()
            .FirstOrDefault(r => r.Id == id);

        return demand is null ? null : MapToDto(demand);
    }

    public ResourceDemandDto Create(CreateResourceDemandRequest request)
    {
        var demand = new ResourceDemand
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            WbsNodeId = request.WbsNodeId,
            RequiredCompetencyId = request.RequiredCompetencyId,
            Title = request.Title,
            Description = request.Description,
            PlannedHours = request.PlannedHours,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = ResourceDemandStatus.Draft,
            CreatedBy = request.CreatedBy,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.ResourceDemands.Add(demand);
        _dbContext.SaveChanges();

        return MapToDto(demand);
    }

    public ResourceDemandDto? Update(Guid id, UpdateResourceDemandRequest request)
    {
        var demand = _dbContext.ResourceDemands.FirstOrDefault(r => r.Id == id);

        if (demand is null)
        {
            return null;
        }

        demand.WbsNodeId = request.WbsNodeId;
        demand.RequiredCompetencyId = request.RequiredCompetencyId;
        demand.Title = request.Title;
        demand.Description = request.Description;
        demand.PlannedHours = request.PlannedHours;
        demand.StartDate = request.StartDate;
        demand.EndDate = request.EndDate;
        demand.Status = request.Status;
        demand.UpdatedBy = request.UpdatedBy;
        demand.UpdatedAtUtc = DateTime.UtcNow;
        demand.DecisionComment = request.DecisionComment;

        _dbContext.SaveChanges();

        return MapToDto(demand);
    }

    private static ResourceDemandDto MapToDto(ResourceDemand demand)
    {
        return new ResourceDemandDto
        {
            Id = demand.Id,
            ProjectId = demand.ProjectId,
            WbsNodeId = demand.WbsNodeId,
            RequiredCompetencyId = demand.RequiredCompetencyId,
            Title = demand.Title,
            Description = demand.Description,
            PlannedHours = demand.PlannedHours,
            StartDate = demand.StartDate,
            EndDate = demand.EndDate,
            Status = demand.Status,
            CreatedBy = demand.CreatedBy,
            CreatedAtUtc = demand.CreatedAtUtc,
            UpdatedBy = demand.UpdatedBy,
            UpdatedAtUtc = demand.UpdatedAtUtc,
            DecisionComment = demand.DecisionComment
        };
    }
}
