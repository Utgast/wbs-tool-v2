using WbsTool.Api.Modules.RateCategories.Contracts;

namespace WbsTool.Api.Modules.RateCategories.Services;

public interface IRateCategoryService
{
    IEnumerable<RateCategoryDto> GetAllActive();
}