using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Projects.Contracts;
using WbsTool.Api.Modules.Projects.Models;

namespace WbsTool.Api.Modules.Projects.Services;

/// <summary>
/// Project Service - Verwaltung von Projekten
/// 
/// Verantwortung:
/// - Projekt CRUD (Create, Read, Update)
/// - Projekt Stammdaten (Name, Beschreibung, Planung)
/// - Project Listing und Details
/// 
/// Wichtig: Projektspezifische WBS-, Ressourcen- und Deliverables-Daten
/// werden durch separate Services verwaltet. Dieser Service kümmert sich
/// nur um Projekt-Metadaten.
/// </summary>
public class ProjectService : IProjectService
{
    private readonly AppDbContext _dbContext;

    public ProjectService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Alle Projekte abrufen (aktiv und inaktiv)
    /// </summary>
    public IEnumerable<ProjectDto> GetAll()
    {
        return _dbContext.Projects
            .AsNoTracking()
            .Select(MapToDto)
            .ToList();
    }

    /// <summary>
    /// Projekt nach ID abrufen
    /// </summary>
    public ProjectDto? GetById(Guid id)
    {
        var project = _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);

        return project is null ? null : MapToDto(project);
    }

    /// <summary>
    /// Neues Projekt erstellen mit Stammdaten
    /// </summary>
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

    /// <summary>
    /// Mappt Project Entity zu DTO für API Response
    /// </summary>
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