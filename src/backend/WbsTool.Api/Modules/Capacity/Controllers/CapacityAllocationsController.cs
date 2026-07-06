using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Capacity.Contracts;
using WbsTool.Api.Modules.Capacity.Services;

namespace WbsTool.Api.Modules.Capacity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CapacityAllocationsController : ControllerBase
{
    private readonly ICapacityAllocationService _capacityAllocationService;

    public CapacityAllocationsController(ICapacityAllocationService capacityAllocationService)
    {
        _capacityAllocationService = capacityAllocationService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CapacityAllocationDto>> GetAll()
    {
        var allocations = _capacityAllocationService.GetAll();
        return Ok(allocations);
    }

    [HttpPost]
    public ActionResult<CapacityAllocationDto> Create([FromBody] CreateCapacityAllocationRequest request)
    {
        var created = _capacityAllocationService.Create(request);
        return Ok(created);
    }
}
