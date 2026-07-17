using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Capacity.Contracts;
using WbsTool.Api.Modules.Capacity.Services;
using WbsTool.Api.Modules.Competencies.Contracts;

namespace WbsTool.Api.Modules.Capacity.Controllers;

[ApiController]
[Route("api/capacity")]
public class CapacityController : ControllerBase
{
    private readonly ICapacityService _capacityService;

    public CapacityController(ICapacityService capacityService)
    {
        _capacityService = capacityService;
    }

    /// <summary>
    /// Liefert alle verfuegbaren Personen fuer Kapazitaetsplanung.
    /// </summary>
    [HttpGet("persons")]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersons()
    {
        var persons = await _capacityService.GetPersonsAsync();
        return Ok(persons);
    }

    /// <summary>
    /// Legt eine Person fuer Kompetenz- und Kapazitaetspflege an.
    /// </summary>
    [HttpPost("persons")]
    public async Task<ActionResult<PersonDto>> CreatePerson(CreatePersonRequest request)
    {
        var person = await _capacityService.CreatePersonAsync(request);
        return Ok(person);
    }

    /// <summary>
    /// Entfernt eine Person aus der bearbeitbaren Stammdatenbasis.
    /// </summary>
    [HttpDelete("persons/{id:guid}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var deleted = await _capacityService.DeletePersonAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Liefert alle gespeicherten Wochenkapazitaeten.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonCapacityDto>>> GetCapacities()
    {
        var capacities = await _capacityService.GetCapacitiesAsync();
        return Ok(capacities);
    }

    /// <summary>
    /// Speichert die verfuegbare Wochenkapazitaet einer Person.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PersonCapacityDto>> Create(CreatePersonCapacityRequest request)
    {
        var capacity = await _capacityService.CreateCapacityAsync(request);
        return Ok(capacity);
    }

    /// <summary>
    /// Loescht einen Wochenkapazitaetseintrag.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _capacityService.DeleteCapacityAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}