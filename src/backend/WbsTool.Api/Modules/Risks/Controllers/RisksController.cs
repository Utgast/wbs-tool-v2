using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Risks.Contracts;
using WbsTool.Api.Modules.Risks.Services;

namespace WbsTool.Api.Modules.Risks.Controllers;

[ApiController]
public class RisksController : ControllerBase
{
    private readonly IRiskService _riskService;

    public RisksController(IRiskService riskService)
    {
        _riskService = riskService;
    }

    [HttpGet("api/projects/{projectId:guid}/risks")]
    public ActionResult<IEnumerable<RiskDto>> GetByProjectId(Guid projectId)
    {
        var risks = _riskService.GetByProjectId(projectId);
        return Ok(risks);
    }

    [HttpGet("api/risks/{id:guid}")]
    public ActionResult<RiskDto> GetById(Guid id)
    {
        var risk = _riskService.GetById(id);

        if (risk is null)
        {
            return NotFound(new
            {
                message = $"Risk with id '{id}' was not found."
            });
        }

        return Ok(risk);
    }

    [HttpPost("api/risks")]
    public ActionResult<RiskDto> Create(CreateRiskRequest request)
    {
        var created = _riskService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    [HttpPut("api/risks/{id:guid}")]
    public ActionResult<RiskDto> Update(Guid id, UpdateRiskRequest request)
    {
        var updated = _riskService.Update(id, request);

        if (updated is null)
        {
            return NotFound(new
            {
                message = $"Risk with id '{id}' was not found."
            });
        }

        return Ok(updated);
    }

    [HttpPatch("api/risks/{id:guid}/close")]
    public ActionResult<RiskDto> Close(Guid id)
    {
        var closed = _riskService.Close(id);

        if (closed is null)
        {
            return NotFound(new
            {
                message = $"Risk with id '{id}' was not found."
            });
        }

        return Ok(closed);
    }

    [HttpGet("api/projects/{projectId:guid}/risks/critical-open-count")]
    public ActionResult<int> CountCriticalOpenByProjectId(Guid projectId)
    {
        var count = _riskService.CountCriticalOpenByProjectId(projectId);
        return Ok(count);
    }
}
