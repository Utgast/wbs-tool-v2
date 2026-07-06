using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.ResourceDemands.Contracts;
using WbsTool.Api.Modules.ResourceDemands.Services;

namespace WbsTool.Api.Modules.ResourceDemands.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourceDemandsController : ControllerBase
{
    private readonly IResourceDemandService _resourceDemandService;

    public ResourceDemandsController(IResourceDemandService resourceDemandService)
    {
        _resourceDemandService = resourceDemandService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ResourceDemandDto>> GetAll()
    {
        var demands = _resourceDemandService.GetAll();
        return Ok(demands);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ResourceDemandDto> GetById(Guid id)
    {
        var demand = _resourceDemandService.GetById(id);

        if (demand is null)
        {
            return NotFound(new
            {
                message = $"ResourceDemand with id '{id}' was not found."
            });
        }

        return Ok(demand);
    }

    [HttpPost]
    public ActionResult<ResourceDemandDto> Create(CreateResourceDemandRequest request)
    {
        var created = _resourceDemandService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<ResourceDemandDto> Update(Guid id, UpdateResourceDemandRequest request)
    {
        var updated = _resourceDemandService.Update(id, request);

        if (updated is null)
        {
            return NotFound(new
            {
                message = $"ResourceDemand with id '{id}' was not found."
            });
        }

        return Ok(updated);
    }
}
