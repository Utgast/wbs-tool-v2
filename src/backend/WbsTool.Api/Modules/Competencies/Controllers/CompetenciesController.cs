using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Competencies.Contracts;
using WbsTool.Api.Modules.Competencies.Services;

namespace WbsTool.Api.Modules.Competencies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetenciesController : ControllerBase
{
    private readonly ICompetencyService _competencyService;

    public CompetenciesController(ICompetencyService competencyService)
    {
        _competencyService = competencyService;
    }

    /// <summary>
    /// Liefert alle Kompetenzen als bearbeitbare Stammdatenliste.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompetencyDto>>> GetAll()
    {
        var competencies = await _competencyService.GetCompetenciesAsync();
        return Ok(competencies);
    }

    /// <summary>
    /// Legt eine neue Kompetenz ohne Bewertungslogik an.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CompetencyDto>> Create(CreateCompetencyRequest request)
    {
        var competency = await _competencyService.CreateCompetencyAsync(request);
        return Ok(competency);
    }

    /// <summary>
    /// Entfernt eine Kompetenz aus der bearbeitbaren Liste.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _competencyService.DeleteCompetencyAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}