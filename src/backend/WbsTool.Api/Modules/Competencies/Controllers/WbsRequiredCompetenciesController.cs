using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Competencies.Contracts;
using WbsTool.Api.Modules.Competencies.Services;

namespace WbsTool.Api.Modules.Competencies.Controllers;

[ApiController]
[Route("api/wbs/{wbsNodeId:guid}/requiredcompetencies")]
public class WbsRequiredCompetenciesController : ControllerBase
{
    private readonly ICompetencyService _competencyService;

    public WbsRequiredCompetenciesController(ICompetencyService competencyService)
    {
        _competencyService = competencyService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WbsRequiredCompetencyDto>> GetByWbsNode(Guid wbsNodeId)
    {
        var competencies = _competencyService.GetWbsRequiredCompetencies(wbsNodeId);
        return Ok(competencies);
    }

    [HttpPost]
    public ActionResult<WbsRequiredCompetencyDto> Add(Guid wbsNodeId, [FromBody] AddWbsRequiredCompetencyRequest request)
    {
        var created = _competencyService.AddWbsRequiredCompetency(wbsNodeId, request);
        return Ok(created);
    }
}
