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

        var dashboardNodes = activeNodes
            .Where(n => n.Level == 1)
            .ToList();

        var totalPlannedHours = dashboardNodes.Sum(n => n.PlannedHours ?? 0m);
        var totalActualHours = dashboardNodes.Sum(n => n.ActualHours ?? 0m);

        var totalPlannedCost = dashboardNodes.Sum(n => n.ImportedPlannedCost ?? 0m);
        var totalActualCost = dashboardNodes.Sum(n => n.ImportedActualCost ?? 0m);

        var progressPercent = 0m;

        if (totalPlannedHours > 0)
        {
            progressPercent = Math.Round(
                (totalActualHours / totalPlannedHours) * 100m,
                2);
        }

        var blockedNodes = activeNodes.Count(n =>
            n.IsBlocked ||
            n.Status != null &&
            (
                n.Status.Code == "Blocked" ||
                n.Status.Code == "Critical"
            ));

        var today = DateOnly.FromDateTime(DateTime.Today);

        var overdueNodes = activeNodes.Count(n =>
            n.PlannedEnd.HasValue &&
            n.PlannedEnd.Value < today &&
            n.Status != null &&
            n.Status.Code != "Delivered" &&
            n.Status.Code != "Done");

        var resourceOverview = GetResourceOverview(projectId);

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
            OverdueNodes = overdueNodes,

            PlannedDemandHours = resourceOverview?.PlannedDemandHours ?? 0m,
            AssignedHours = resourceOverview?.AssignedHours ?? 0m,
            OpenHours = resourceOverview?.OpenHours ?? 0m,
            CapacityHours = resourceOverview?.CapacityHours ?? 0m,
            UtilizationPercent = resourceOverview?.UtilizationPercent ?? 0m
        };
    }

    public ProjectResourceOverviewDto? GetResourceOverview(Guid projectId)
    {
        var projectExists = _dbContext.Projects
            .AsNoTracking()
            .Any(p => p.Id == projectId && p.IsActive);

        if (!projectExists)
        {
            return null;
        }

        var plannedDemandHours = _dbContext.ResourceDemands
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId)
            .Select(x => x.PlannedHours ?? 0m)
            .ToList()
            .Sum();

        var assignedHours = (
            from assignment in _dbContext.ResourceAssignments.AsNoTracking()
            join wbsNode in _dbContext.WbsNodes.AsNoTracking()
                on assignment.WbsNodeId equals wbsNode.Id
            where wbsNode.ProjectId == projectId
                  && assignment.IsActive
                  && wbsNode.IsActive
            select assignment.PlannedHours ?? 0m
        )
        .ToList()
        .Sum();

        var capacityHours = _dbContext.CapacityAllocations
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.IsActive)
            .Select(x => x.PlannedHours ?? 0m)
            .ToList()
            .Sum();

        var openHours = plannedDemandHours - assignedHours;

        var utilizationPercent = 0m;

        if (capacityHours > 0)
        {
            utilizationPercent = Math.Round(
                assignedHours / capacityHours * 100m,
                2);
        }

        return new ProjectResourceOverviewDto
        {
            PlannedDemandHours = plannedDemandHours,
            AssignedHours = assignedHours,
            OpenHours = openHours,
            CapacityHours = capacityHours,
            UtilizationPercent = utilizationPercent
        };
    }
}