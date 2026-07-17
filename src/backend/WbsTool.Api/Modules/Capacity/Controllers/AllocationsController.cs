using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Capacity.Contracts;
using WbsTool.Api.Modules.Capacity.Services;

namespace WbsTool.Api.Modules.Capacity.Controllers;

[ApiController]
[Route("api/allocations")]
public class AllocationsController : ControllerBase
{
    private readonly ICapacityService _capacityService;

    public AllocationsController(ICapacityService capacityService)
    {
        _capacityService = capacityService;
    }

    /// <summary>
    /// Liefert alle geplanten Stundenallokationen.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonAllocationDto>>> GetAll()
    {
        var allocations = await _capacityService.GetAllocationsAsync();
        return Ok(allocations);
    }

    /// <summary>
    /// Speichert eine neue Stundenallokation einer Person.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PersonAllocationDto>> Create(CreatePersonAllocationRequest request)
    {
        var allocation = await _capacityService.CreateAllocationAsync(request);
        return Ok(allocation);
    }

    /// <summary>
    /// Loescht eine bestehende Stundenallokation.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _capacityService.DeleteAllocationAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}