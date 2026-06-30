using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Persons.Contracts;

namespace WbsTool.Api.Modules.Persons.Services;

public class PersonService : IPersonService
{
    private readonly AppDbContext _dbContext;

    public PersonService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<PersonDto> GetAllActive()
    {
        return _dbContext.Persons
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayName)
            .Select(p => new PersonDto
            {
                Id = p.Id,
                DisplayName = p.DisplayName,
                ShortName = p.ShortName,
                Email = p.Email,
                IsPlaceholder = p.IsPlaceholder,
                PlaceholderType = p.PlaceholderType,
                IsActive = p.IsActive
            })
            .ToList();
    }
}