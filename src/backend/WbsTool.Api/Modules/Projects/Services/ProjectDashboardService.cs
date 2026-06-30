using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Projects.Contracts;

namespace WbsTool.Api.Modules.Projects.Services;

public class ProjectDashboardService : IProjectDashboardService
{
    private readonly AppDbContext _dbContext;

    public ProjectDashboardService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ProjectDashboardDto? GetDashboard(Guid projectId)
    {
        var project = _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == projectId && p.IsActive);

        if (project is null)
        {
            return null;
        }

        var activeNodes = _dbContext.WbsNodes
            .AsNoTracking()
            .Include(w => w.Status)
            .Where(w => w.ProjectId == projectId && w.IsActive)
            .ToList();

        /*
         * Wichtig:
         * Für das Dashboard verwenden wir in Phase 1 bewusst die konsolidierten
         * WBS-Knotenwerte und NICHT die Summe aller ResourceAssignments.
         *
         * Grund:
         * Die alten Excel-Daten enthalten mehrere Ressourcen-Zeilen pro WBS-ID.
         * Das ist für die Detailansicht richtig, führt im Dashboard aber zu
         * Doppel-/Mehrfachzählungen.
         *
         * Deshalb:
         * Dashboard = Level-1-WBS-Werte
         * Ressourcenbereich = direkte ResourceAssignments
         */
        var dashboardNodes = activeNodes
            .Where(n => n.Level == 1)
            .ToList();

        var totalPlannedHours = dashboardNodes.Sum(n => n.PlannedHours ?? 0m);
        var totalActualHours = dashboardNodes.Sum(n => n.ActualHours ?? 0m);

        /*
         * Kosten:
         * In Phase 1 verwenden wir nur importierte/konsolidierte Kosten,
         * falls diese am WBS-Knoten vorhanden sind.
         *
         * Wenn ImportedPlannedCost / ImportedActualCost noch nicht befüllt sind,
         * bleiben die Kosten im Dashboard 0.
         *
         * Das ist bewusst besser, als falsche Kosten aus mehrfachen
         * Ressourcen-Zuordnungen zusammenzurechnen.
         */
        var totalPlannedCost = dashboardNodes.Sum(n => n.ImportedPlannedCost ?? 0m);
        var totalActualCost = dashboardNodes.Sum(n => n.ImportedActualCost ?? 0m);

        var progressPercent = 0m;

        if (totalPlannedHours > 0)
        {
            progressPercent = Math.Round((totalActualHours / totalPlannedHours) * 100m, 2);
        }

        var blockedNodes = activeNodes.Count(n =>
            n.IsBlocked ||
            n.Status != null &&
            (
                n.Status.Code == "Blocked" ||
                n.Status.Code == "Critical"
            ));

        var today = DateOnly.FromDateTime(DateTime.Today);

        /*
         * Überfällige Knoten:
         * Ein Knoten gilt nur dann als überfällig, wenn:
         * - ein Planende vorhanden ist
         * - das Planende vor heute liegt
         * - der Knoten nicht geliefert/abgeschlossen ist
         *
         * Dadurch werden bereits gelieferte Alt-Termine nicht unnötig als offen überfällig gezählt.
         */
        var overdueNodes = activeNodes.Count(n =>
            n.PlannedEnd.HasValue &&
            n.PlannedEnd.Value < today &&
            n.Status != null &&
            n.Status.Code != "Delivered" &&
            n.Status.Code != "Done");

        return new ProjectDashboardDto
        {
            ProjectId = project.Id,
            ProjectName = project.Name,
            TotalPlannedHours = totalPlannedHours,
            TotalActualHours = totalActualHours,
            TotalPlannedCost = totalPlannedCost,
            TotalActualCost = totalActualCost,
            ProgressPercent = progressPercent,
            BlockedNodes = blockedNodes,
            OverdueNodes = overdueNodes
        };
    }
}