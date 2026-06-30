using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Wbs.Contracts;
using WbsTool.Api.Modules.Wbs.Services;

namespace WbsTool.Api.Modules.Wbs.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/wbs/{wbsNodeId:guid}/assignments")]
public class ResourceAssignmentsController : ControllerBase
{
    private readonly IResourceAssignmentService _resourceAssignmentService;

    public ResourceAssignmentsController(IResourceAssignmentService resourceAssignmentService)
    {
        _resourceAssignmentService = resourceAssignmentService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ResourceAssignmentDto>> GetByWbsNode(Guid projectId, Guid wbsNodeId)
    {
        var assignments = _resourceAssignmentService.GetByWbsNode(projectId, wbsNodeId);
        return Ok(assignments);
    }

    [HttpPost]
    public ActionResult<ResourceAssignmentDto> Create(
        Guid projectId,
        Guid wbsNodeId,
        [FromBody] CreateResourceAssignmentRequest request)
    {
        try
        {
            var created = _resourceAssignmentService.Create(projectId, wbsNodeId, request);
            return Ok(created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("{assignmentId:guid}")]
    public ActionResult<ResourceAssignmentDto> Update(
        Guid projectId,
        Guid wbsNodeId,
        Guid assignmentId,
        [FromBody] UpdateResourceAssignmentRequest request)
    {
        try
        {
            var updated = _resourceAssignmentService.Update(projectId, wbsNodeId, assignmentId, request);

            if (updated is null)
            {
                return NotFound(new
                {
                    message = $"Resource assignment with id '{assignmentId}' was not found for WBS node '{wbsNodeId}' in project '{projectId}'."
                });
            }

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{assignmentId:guid}")]
    public IActionResult Deactivate(Guid projectId, Guid wbsNodeId, Guid assignmentId)
    {
        var deactivated = _resourceAssignmentService.Deactivate(projectId, wbsNodeId, assignmentId);

        if (!deactivated)
        {
            return NotFound(new
            {
                message = $"Resource assignment with id '{assignmentId}' was not found for WBS node '{wbsNodeId}' in project '{projectId}'."
            });
        }

        return NoContent();
    }
}