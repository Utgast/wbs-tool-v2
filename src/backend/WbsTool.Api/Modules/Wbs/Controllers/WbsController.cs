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
        try
        {
            var createdNode = _wbsService.Create(projectId, request);
            return Ok(createdNode);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("{id:guid}")]
    public ActionResult<WbsNodeDto> Update(Guid projectId, Guid id, UpdateWbsNodeRequest request)
    {
        try
        {
            var updatedNode = _wbsService.Update(projectId, id, request);

            if (updatedNode is null)
            {
                return NotFound(new
                {
                    message = $"WBS node with id '{id}' was not found in project '{projectId}'."
                });
            }

            return Ok(updatedNode);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult SoftDelete(Guid id)
    {
        var deleted = _wbsService.SoftDelete(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"WBS node with id '{id}' was not found."
            });
        }

        return NoContent();
    }
}