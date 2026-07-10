using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Deliverables.Contracts;
using WbsTool.Api.Modules.Deliverables.Services;

namespace WbsTool.Api.Modules.Deliverables.Controllers;

[ApiController]
public class DeliverablesController : ControllerBase
{
    private readonly IDeliverableService _deliverableService;

    public DeliverablesController(IDeliverableService deliverableService)
    {
        _deliverableService = deliverableService;
    }

    [HttpGet("api/projects/{projectId:guid}/deliverables")]
    public ActionResult<IEnumerable<DeliverableDto>> GetByProjectId(Guid projectId)
    {
        var deliverables = _deliverableService.GetByProjectId(projectId);
        return Ok(deliverables);
    }

    [HttpGet("api/deliverables/{id:guid}")]
    public ActionResult<DeliverableDto> GetById(Guid id)
    {
        var deliverable = _deliverableService.GetById(id);

        if (deliverable is null)
        {
            return NotFound(new
            {
                message = $"Deliverable with id '{id}' was not found."
            });
        }

        return Ok(deliverable);
    }

    [HttpPost("api/deliverables")]
    public ActionResult<DeliverableDto> Create(CreateDeliverableRequest request)
    {
        var created = _deliverableService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    [HttpPut("api/deliverables/{id:guid}")]
    public ActionResult<DeliverableDto> Update(Guid id, UpdateDeliverableRequest request)
    {
        var updated = _deliverableService.Update(id, request);

        if (updated is null)
        {
            return NotFound(new
            {
                message = $"Deliverable with id '{id}' was not found."
            });
        }

        return Ok(updated);
    }

    [HttpPatch("api/deliverables/{id:guid}/deliver")]
    public ActionResult<DeliverableDto> MarkDelivered(Guid id)
    {
        var delivered = _deliverableService.MarkDelivered(id);

        if (delivered is null)
        {
            return NotFound(new
            {
                message = $"Deliverable with id '{id}' was not found."
            });
        }

        return Ok(delivered);
    }

    [HttpGet("api/projects/{projectId:guid}/deliverables/overdue-count")]
    public ActionResult<int> CountOverdueByProjectId(Guid projectId)
    {
        var count = _deliverableService.CountOverdueByProjectId(projectId);
        return Ok(count);
    }
}
