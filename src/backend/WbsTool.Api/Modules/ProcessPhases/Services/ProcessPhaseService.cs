using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.ProcessPhases.Contracts;

namespace WbsTool.Api.Modules.ProcessPhases.Services;

public class ProcessPhaseService : IProcessPhaseService
{
    private readonly AppDbContext _dbContext;

    public ProcessPhaseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<ProcessPhaseDto> GetAll()
    {
        return _dbContext.ProcessPhases
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .Select(MapToDto)
            .ToList();
    }

    public IEnumerable<ProcessPhaseDto> GetByProjectId(Guid projectId)
    {
        return _dbContext.WbsPhaseMappings
            .AsNoTracking()
            .Where(m => m.ProjectId == projectId)
            .Include(m => m.ProcessPhase)
            .Select(m => m.ProcessPhase)
            .Where(p => p.IsActive)
            .Distinct()
            .OrderBy(p => p.SortOrder)
            .Select(MapToDto)
            .ToList();
    }

    private static ProcessPhaseDto MapToDto(Models.ProcessPhase phase)
    {
        return new ProcessPhaseDto
        {
            Id = phase.Id,
            Code = phase.Code,
            Name = phase.Name,
            Goal = phase.Goal,
            Description = phase.Description,
            DefaultResponsibility = phase.DefaultResponsibility,
            SortOrder = phase.SortOrder,
            IsActive = phase.IsActive
        };
    }
}
