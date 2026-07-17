using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Dashboard.Contracts;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Dashboard.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _dbContext;

    public DashboardService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Ermittelt die Dashboard-MVP-Kennzahlen auf Basis der vorhandenen Projects- und WBS-Daten.
    /// </summary>
    public async Task<DashboardDto> GetDashboardAsync()
    {
        // Kennzahl fuer Management-Sicht: zeigt, wie viele aktive Vorhaben aktuell gesteuert werden.
        var projectsCount = await _dbContext.Projects
            .AsNoTracking()
            .CountAsync(p => p.IsActive);

        // Kennzahl fuer Arbeitsumfang: zeigt die Gesamtmenge der zu steuernden Arbeitspakete.
        var workPackagesCount = await _dbContext.WbsNodes
            .AsNoTracking()
            .CountAsync();

        // Kennzahl fuer Status-Transparenz: offene Pakete bilden den verbleibenden Umsetzungsumfang.
        var openWorkPackagesCount = await _dbContext.WbsNodes
            .AsNoTracking()
            .CountAsync(w => w.Status == WbsNodeStatus.Offen);

        // Kennzahl fuer Eskalation: blockierte Pakete markieren sofortigen Handlungsbedarf.
        var blockedWorkPackagesCount = await _dbContext.WbsNodes
            .AsNoTracking()
            .CountAsync(w => w.Status == WbsNodeStatus.Blockiert);

        // Kennzahl fuer Projektlage: Durchschnitt ueber ProgressPercent liefert den Gesamtfortschritt.
        var projectProgressPercent = await _dbContext.WbsNodes
            .AsNoTracking()
            .Select(w => (decimal?)w.ProgressPercent)
            .AverageAsync() ?? 0m;

        // Kritische Pakete sind hier als in Bearbeitung mit sehr niedrigem Fortschritt definiert,
        // damit frueh reagiert werden kann, bevor ein Blocker formal gesetzt wird.
        var criticalWorkPackagesCount = await _dbContext.WbsNodes
            .AsNoTracking()
            .CountAsync(w => w.Status == WbsNodeStatus.InBearbeitung && w.ProgressPercent < 30);

        // Handlungsbedarf kombiniert harte Blockade und fruehe Kritikalitaet,
        // damit die Priorisierung im Management-Dashboard unmittelbar sichtbar ist.
        var actionRequiredCount = blockedWorkPackagesCount + criticalWorkPackagesCount;

        return new DashboardDto
        {
            ProjectsCount = projectsCount,
            WorkPackagesCount = workPackagesCount,
            OpenWorkPackagesCount = openWorkPackagesCount,
            BlockedWorkPackagesCount = blockedWorkPackagesCount,
            ProjectProgressPercent = Math.Round(projectProgressPercent, 2, MidpointRounding.AwayFromZero),
            ActionRequiredCount = actionRequiredCount
        };
    }
}
