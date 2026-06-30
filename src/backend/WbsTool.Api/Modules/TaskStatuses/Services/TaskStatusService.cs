using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.TaskStatuses.Contracts;

namespace WbsTool.Api.Modules.TaskStatuses.Services;

public class TaskStatusService : ITaskStatusService
{
    private readonly AppDbContext _dbContext;

    public TaskStatusService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<TaskStatusDto> GetAllActive()
    {
        return _dbContext.TaskStatuses
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.SortOrder)
            .ThenBy(s => s.Label)
            .Select(s => new TaskStatusDto
            {
                Id = s.Id,
                Code = s.Code,
                Label = s.Label,
                Color = s.Color,
                SortOrder = s.SortOrder,
                IsActive = s.IsActive,
                IsTerminal = s.IsTerminal
            })
            .ToList();
    }
}