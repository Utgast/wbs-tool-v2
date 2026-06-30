using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.RateCategories.Contracts;

namespace WbsTool.Api.Modules.RateCategories.Services;

public class RateCategoryService : IRateCategoryService
{
    private readonly AppDbContext _dbContext;

    public RateCategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<RateCategoryDto> GetAllActive()
    {
        return _dbContext.RateCategories
            .AsNoTracking()
            .Where(r => r.IsActive)
            .OrderBy(r => r.Code)
            .Select(r => new RateCategoryDto
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Name,
                HourlyRate = r.HourlyRate,
                Currency = r.Currency,
                IsActive = r.IsActive
            })
            .ToList();
    }
}