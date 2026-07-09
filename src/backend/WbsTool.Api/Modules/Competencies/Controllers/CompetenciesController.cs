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

    [HttpGet]
    public ActionResult<IEnumerable<CompetencyDto>> GetAll()
    {
        var competencies = _competencyService.GetAll();
        return Ok(competencies);
    }
        [HttpGet("{competencyId:guid}/persons")]
    public ActionResult<IEnumerable<CompetencyPersonDto>> GetPersonsByCompetency(Guid competencyId)
    {
        var persons = _competencyService.GetPersonsByCompetency(competencyId);
        return Ok(persons);
    }
}
