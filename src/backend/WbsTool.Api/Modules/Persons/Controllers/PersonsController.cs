using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Persons.Contracts;
using WbsTool.Api.Modules.Persons.Services;

namespace WbsTool.Api.Modules.Persons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PersonDto>> GetAllActive()
    {
        var persons = _personService.GetAllActive();
        return Ok(persons);
    }
}