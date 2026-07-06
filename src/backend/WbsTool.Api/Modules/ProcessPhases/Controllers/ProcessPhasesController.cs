using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.ProcessPhases.Contracts;
using WbsTool.Api.Modules.ProcessPhases.Services;

namespace WbsTool.Api.Modules.ProcessPhases.Controllers;

[ApiController]
public class ProcessPhasesController : ControllerBase
{
    private readonly IProcessPhaseService _processPhaseService;

    public ProcessPhasesController(IProcessPhaseService processPhaseService)
    {
        _processPhaseService = processPhaseService;
    }

    [HttpGet("api/ProcessPhases")]
    public ActionResult<IEnumerable<ProcessPhaseDto>> GetAll()
    {
        var phases = _processPhaseService.GetAll();
        return Ok(phases);
    }

    [HttpGet("api/Projects/{projectId:guid}/ProcessPhases")]
    public ActionResult<IEnumerable<ProcessPhaseDto>> GetByProjectId(Guid projectId)
    {
        var phases = _processPhaseService.GetByProjectId(projectId);
        return Ok(phases);
    }
}
