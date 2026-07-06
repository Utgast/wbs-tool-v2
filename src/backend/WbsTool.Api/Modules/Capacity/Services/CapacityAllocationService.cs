using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Capacity.Contracts;
using WbsTool.Api.Modules.Capacity.Models;

namespace WbsTool.Api.Modules.Capacity.Services;

public class CapacityAllocationService : ICapacityAllocationService
{
    private readonly AppDbContext _dbContext;

    public CapacityAllocationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<CapacityAllocationDto> GetAll()
    {
        return _dbContext.CapacityAllocations
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.StartDate)
            .Select(MapToDto)
            .ToList();
    }

    public CapacityAllocationDto Create(CreateCapacityAllocationRequest request)
    {
        var allocation = new CapacityAllocation
        {
            Id = Guid.NewGuid(),
            PersonId = request.PersonId,
            ProjectId = request.ProjectId,
            WbsNodeId = request.WbsNodeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PlannedHours = request.PlannedHours,
            ActualHours = request.ActualHours,
            AllocationPercent = request.AllocationPercent,
            Source = request.Source,
            Comment = request.Comment,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _dbContext.CapacityAllocations.Add(allocation);
        _dbContext.SaveChanges();

        return MapToDto(allocation);
    }

    private static CapacityAllocationDto MapToDto(CapacityAllocation allocation)
    {
        return new CapacityAllocationDto
        {
            Id = allocation.Id,
            PersonId = allocation.PersonId,
            ProjectId = allocation.ProjectId,
            WbsNodeId = allocation.WbsNodeId,
            StartDate = allocation.StartDate,
            EndDate = allocation.EndDate,
            PlannedHours = allocation.PlannedHours,
            ActualHours = allocation.ActualHours,
            AllocationPercent = allocation.AllocationPercent,
            Source = allocation.Source,
            Comment = allocation.Comment,
            IsActive = allocation.IsActive
        };
    }
}
