using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Deliverables.Contracts;
using WbsTool.Api.Modules.Deliverables.Models;

namespace WbsTool.Api.Modules.Deliverables.Services;

public class DeliverableService : IDeliverableService
{
    private readonly AppDbContext _dbContext;

    public DeliverableService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<DeliverableDto> GetByProjectId(Guid projectId)
    {
        return _dbContext.Deliverables
            .AsNoTracking()
            .Where(d => d.ProjectId == projectId)
            .OrderBy(d => d.CreatedAt)
            .Select(MapToDto)
            .ToList();
    }

    public DeliverableDto? GetById(Guid id)
    {
        var deliverable = _dbContext.Deliverables
            .AsNoTracking()
            .FirstOrDefault(d => d.Id == id);

        return deliverable is null ? null : MapToDto(deliverable);
    }

    public DeliverableDto Create(CreateDeliverableRequest request)
    {
        var deliverable = new Deliverable
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            Status = DeliverableStatus.Draft,
            OwnerPersonId = request.OwnerPersonId,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            ProjectId = request.ProjectId,
            ProcessPhaseId = request.ProcessPhaseId,
            WbsNodeId = request.WbsNodeId
        };

        _dbContext.Deliverables.Add(deliverable);
        _dbContext.SaveChanges();

        return MapToDto(deliverable);
    }

    public DeliverableDto? Update(Guid id, UpdateDeliverableRequest request)
    {
        var deliverable = _dbContext.Deliverables.FirstOrDefault(d => d.Id == id);

        if (deliverable is null)
        {
            return null;
        }

        deliverable.Name = request.Name;
        deliverable.Description = request.Description;
        deliverable.Type = request.Type;
        deliverable.Status = request.Status;
        deliverable.OwnerPersonId = request.OwnerPersonId;
        deliverable.DueDate = request.DueDate;
        deliverable.ProcessPhaseId = request.ProcessPhaseId;
        deliverable.WbsNodeId = request.WbsNodeId;

        _dbContext.SaveChanges();

        return MapToDto(deliverable);
    }

    public DeliverableDto? MarkDelivered(Guid id)
    {
        var deliverable = _dbContext.Deliverables.FirstOrDefault(d => d.Id == id);

        if (deliverable is null)
        {
            return null;
        }

        deliverable.Status = DeliverableStatus.Delivered;

        _dbContext.SaveChanges();

        return MapToDto(deliverable);
    }

    public int CountOverdueByProjectId(Guid projectId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return _dbContext.Deliverables
            .AsNoTracking()
            .Count(d =>
                d.ProjectId == projectId &&
                d.DueDate < today &&
                d.Status != DeliverableStatus.Delivered);
    }

    private static DeliverableDto MapToDto(Deliverable deliverable)
    {
        return new DeliverableDto
        {
            Id = deliverable.Id,
            Name = deliverable.Name,
            Description = deliverable.Description,
            Type = deliverable.Type,
            Status = deliverable.Status,
            OwnerPersonId = deliverable.OwnerPersonId,
            DueDate = deliverable.DueDate,
            CreatedAt = deliverable.CreatedAt,
            ProjectId = deliverable.ProjectId,
            ProcessPhaseId = deliverable.ProcessPhaseId,
            WbsNodeId = deliverable.WbsNodeId
        };
    }
}
