using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Projects.Contracts;
using WbsTool.Api.Modules.Projects.Services;

namespace WbsTool.Api.Modules.Projects.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IProjectDashboardService _projectDashboardService;

    public ProjectsController(
        IProjectService projectService,
        IProjectDashboardService projectDashboardService)
    {
        _projectService = projectService;
        _projectDashboardService = projectDashboardService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProjectDto>> GetAll()
    {
        var projects = _projectService.GetAll();
        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProjectDto> GetById(Guid id)
    {
        var project = _projectService.GetById(id);

        if (project is null)
        {
            return NotFound(new
            {
                message = $"Project with id '{id}' was not found."
            });
        }

        return Ok(project);
    }

    [HttpGet("{id:guid}/dashboard")]
    public ActionResult<ProjectDashboardDto> GetDashboard(Guid id)
    {
        var dashboard = _projectDashboardService.GetDashboard(id);

        if (dashboard is null)
        {
            return NotFound(new
            {
                message = $"Project with id '{id}' was not found."
            });
        }

        return Ok(dashboard);
    }

    [HttpPost]
    public ActionResult<ProjectDto> Create(CreateProjectRequest request)
    {
        var createdProject = _projectService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProject.Id },
            createdProject);
    }
}