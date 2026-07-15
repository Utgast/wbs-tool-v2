using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Wbs.Contracts;
using WbsTool.Api.Modules.Wbs.Services;

namespace WbsTool.Api.Modules.Wbs.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/wbs")]
public class WbsController : ControllerBase
{
    private readonly IWbsService _wbsService;

    public WbsController(IWbsService wbsService)
    {
        _wbsService = wbsService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WbsNodeDto>> GetByProjectId(Guid projectId)
    {
        var nodes = _wbsService.GetByProjectId(projectId);
        return Ok(nodes);
    }

    [HttpGet("tree")]
    public ActionResult<IEnumerable<WbsTreeNodeDto>> GetTreeByProjectId(Guid projectId)
    {
        var nodes = _wbsService.GetTreeByProjectId(projectId);
        return Ok(nodes);
    }

    [HttpPost]
    public ActionResult<WbsNodeDto> Create(Guid projectId, CreateWbsNodeRequest request)
    {
        var createdNode = _wbsService.Create(projectId, request);
        return Ok(createdNode);
    }
}