using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Seed.Services;

namespace WbsTool.Api.Modules.Seed.Controllers;

[ApiController]
[Route("api/dev-seed")]
public class DevSeedController : ControllerBase
{
    private readonly IAmprionPqSeedService _seedService;
    private readonly IWebHostEnvironment _environment;

    public DevSeedController(
        IAmprionPqSeedService seedService,
        IWebHostEnvironment environment)
    {
        _seedService = seedService;
        _environment = environment;
    }

    [HttpPost("amprion-pq")]
    public async Task<ActionResult<AmprionPqSeedResultDto>> SeedAmprionPq(
        CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
        {
            return Forbid();
        }

        var result = await _seedService.SeedAsync(cancellationToken);

        return Ok(result);
    }
}