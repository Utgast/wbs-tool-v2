using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Projects.Contracts;
using WbsTool.Api.Modules.Projects.Models;

namespace WbsTool.Api.Modules.Projects.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _dbContext;

    public ProjectService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<ProjectDto> GetAll()
    {
        return _dbContext.Projects
            .AsNoTracking()
            .Select(MapToDto)
            .ToList();
    }

    public ProjectDto? GetById(Guid id)
    {
        var project = _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);

        return project is null ? null : MapToDto(project);
    }

    public ProjectDto Create(CreateProjectRequest request)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            ProjectNumber = request.ProjectNumber,
            Name = request.Name,
            Description = request.Description,
            PlannedStart = request.PlannedStart,
            PlannedEnd = request.PlannedEnd,
            IsActive = true
        };

        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();

        return MapToDto(project);
    }

    private static ProjectDto MapToDto(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            ProjectNumber = project.ProjectNumber,
            Name = project.Name,
            Description = project.Description,
            PlannedStart = project.PlannedStart,
            PlannedEnd = project.PlannedEnd,
            IsActive = project.IsActive
        };
    }
}