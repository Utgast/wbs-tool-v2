using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Competencies.Contracts;
using WbsTool.Api.Modules.Competencies.Services;

namespace WbsTool.Api.Modules.Competencies.Controllers;

[ApiController]
[Route("api/person-competencies")]
public class PersonCompetenciesController : ControllerBase
{
    private readonly ICompetencyService _competencyService;

    public PersonCompetenciesController(ICompetencyService competencyService)
    {
        _competencyService = competencyService;
    }

    /// <summary>
    /// Liefert alle Zuordnungen zwischen Personen und Kompetenzen.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonCompetencyDto>>> GetAll()
    {
        var assignments = await _competencyService.GetPersonCompetenciesAsync();
        return Ok(assignments);
    }

    /// <summary>
    /// Legt eine neue Zuordnung Person zu Kompetenz an.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PersonCompetencyDto>> Create(CreatePersonCompetencyRequest request)
    {
        var assignment = await _competencyService.CreatePersonCompetencyAsync(request);
        return Ok(assignment);
    }

    /// <summary>
    /// Loescht eine bestehende Zuordnung zwischen Person und Kompetenz.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _competencyService.DeletePersonCompetencyAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}