using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.RateCategories.Contracts;
using WbsTool.Api.Modules.RateCategories.Services;

namespace WbsTool.Api.Modules.RateCategories.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RateCategoriesController : ControllerBase
{
    private readonly IRateCategoryService _rateCategoryService;

    public RateCategoriesController(IRateCategoryService rateCategoryService)
    {
        _rateCategoryService = rateCategoryService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RateCategoryDto>> GetAllActive()
    {
        var rateCategories = _rateCategoryService.GetAllActive();
        return Ok(rateCategories);
    }
}