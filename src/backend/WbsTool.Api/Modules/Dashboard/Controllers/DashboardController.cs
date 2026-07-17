using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Dashboard.Contracts;
using WbsTool.Api.Modules.Dashboard.Services;

namespace WbsTool.Api.Modules.Dashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    /// <summary>
    /// Liefert die Dashboard-MVP-Kennzahlen als kompakte Management-Uebersicht.
    /// </summary>
    // Fachlicher Zweck: Der Endpunkt buendelt den aktuellen Projektzustand,
    // damit Priorisierung und kurzfristiger Handlungsbedarf sofort sichtbar sind.
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> Get([FromServices] AppDbContext dbContext)
    {
        IDashboardService dashboardService = new DashboardService(dbContext);
        var dashboard = await dashboardService.GetDashboardAsync();
        return Ok(dashboard);
    }
}
