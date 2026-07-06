using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Competencies.Contracts;
using WbsTool.Api.Modules.Competencies.Services;

namespace WbsTool.Api.Modules.Competencies.Controllers;

[ApiController]
[Route("api/persons/{personId:guid}/competencies")]
public class PersonCompetenciesController : ControllerBase
{
    private readonly ICompetencyService _competencyService;

    public PersonCompetenciesController(ICompetencyService competencyService)
    {
        _competencyService = competencyService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PersonCompetencyDto>> GetByPerson(Guid personId)
    {
        var competencies = _competencyService.GetPersonCompetencies(personId);
        return Ok(competencies);
    }

    [HttpPost]
    public ActionResult<PersonCompetencyDto> Add(Guid personId, [FromBody] AddPersonCompetencyRequest request)
    {
        var created = _competencyService.AddPersonCompetency(personId, request);
        return Ok(created);
    }
}
