using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Risks.Contracts;
using WbsTool.Api.Modules.Risks.Models;

namespace WbsTool.Api.Modules.Risks.Services;

public class RiskService : IRiskService
{
    private readonly AppDbContext _dbContext;

    public RiskService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<RiskDto> GetByProjectId(Guid projectId)
    {
        return _dbContext.Risks
            .AsNoTracking()
            .Where(r => r.ProjectId == projectId)
            .OrderBy(r => r.CreatedAt)
            .Select(MapToDto)
            .ToList();
    }

    public RiskDto? GetById(Guid id)
    {
        var risk = _dbContext.Risks
            .AsNoTracking()
            .FirstOrDefault(r => r.Id == id);

        return risk is null ? null : MapToDto(risk);
    }

    public RiskDto Create(CreateRiskRequest request)
    {
        var risk = new Risk
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Severity = request.Severity,
            Status = RiskStatus.New,
            OwnerPersonId = request.OwnerPersonId,
            ProjectId = request.ProjectId,
            CreatedAt = DateTime.UtcNow,
            DueDate = request.DueDate,
            WbsNodeId = request.WbsNodeId
        };

        _dbContext.Risks.Add(risk);
        _dbContext.SaveChanges();

        return MapToDto(risk);
    }

    public RiskDto? Update(Guid id, UpdateRiskRequest request)
    {
        var risk = _dbContext.Risks.FirstOrDefault(r => r.Id == id);

        if (risk is null)
        {
            return null;
        }

        risk.Title = request.Title;
        risk.Description = request.Description;
        risk.Category = request.Category;
        risk.Severity = request.Severity;
        risk.Status = request.Status;
        risk.OwnerPersonId = request.OwnerPersonId;
        risk.DueDate = request.DueDate;
        risk.WbsNodeId = request.WbsNodeId;

        _dbContext.SaveChanges();

        return MapToDto(risk);
    }

    public RiskDto? Close(Guid id)
    {
        var risk = _dbContext.Risks.FirstOrDefault(r => r.Id == id);

        if (risk is null)
        {
            return null;
        }

        risk.Status = RiskStatus.Closed;

        _dbContext.SaveChanges();

        return MapToDto(risk);
    }

    public int CountCriticalOpenByProjectId(Guid projectId)
    {
        return _dbContext.Risks
            .AsNoTracking()
            .Count(r =>
                r.ProjectId == projectId &&
                r.Severity == RiskSeverity.High &&
                r.Status != RiskStatus.Accepted &&
                r.Status != RiskStatus.Closed);
    }

    private static RiskDto MapToDto(Risk risk)
    {
        return new RiskDto
        {
            Id = risk.Id,
            Title = risk.Title,
            Description = risk.Description,
            Category = risk.Category,
            Severity = risk.Severity,
            Status = risk.Status,
            OwnerPersonId = risk.OwnerPersonId,
            ProjectId = risk.ProjectId,
            CreatedAt = risk.CreatedAt,
            DueDate = risk.DueDate,
            WbsNodeId = risk.WbsNodeId
        };
    }
}
