using WbsTool.Api.Modules.Persons.Contracts;

namespace WbsTool.Api.Modules.Persons.Services;

public interface IPersonService
{
    IEnumerable<PersonDto> GetAllActive();
}