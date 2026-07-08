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
        var competencyOverview = GetCompetencyOverview(projectId);

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
            UtilizationPercent = resourceOverview?.UtilizationPercent ?? 0m,

            RequiredCompetencies = competencyOverview.RequiredCompetencies,
            CoveredCompetencies = competencyOverview.CoveredCompetencies,
            MissingCompetencies = competencyOverview.MissingCompetencies,
            CompetencyCoveragePercent = competencyOverview.CompetencyCoveragePercent
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

    private ProjectCompetencyOverview GetCompetencyOverview(Guid projectId)
    {
        var requiredCompetencyIds = (
            from requiredCompetency in _dbContext.WbsRequiredCompetencies.AsNoTracking()
            join wbsNode in _dbContext.WbsNodes.AsNoTracking()
                on requiredCompetency.WbsNodeId equals wbsNode.Id
            where requiredCompetency.ProjectId == projectId
                  && wbsNode.IsActive
            select requiredCompetency.CompetencyId
        )
        .Distinct()
        .ToList();

        var availableCompetencyIds = (
            from personCompetency in _dbContext.PersonCompetencies.AsNoTracking()
            join person in _dbContext.Persons.AsNoTracking()
                on personCompetency.PersonId equals person.Id
            where personCompetency.IsActive
                  && person.IsActive
            select personCompetency.CompetencyId
        )
        .Distinct()
        .ToList();

        var coveredCompetencies = requiredCompetencyIds
            .Intersect(availableCompetencyIds)
            .Count();

        var missingCompetencies = requiredCompetencyIds
            .Except(availableCompetencyIds)
            .Count();

        var requiredCompetencies = requiredCompetencyIds.Count;

        var competencyCoveragePercent = 0m;

        if (requiredCompetencies > 0)
        {
            competencyCoveragePercent = Math.Round(
                (decimal)coveredCompetencies / requiredCompetencies * 100m,
                2);
        }

        return new ProjectCompetencyOverview
        {
            RequiredCompetencies = requiredCompetencies,
            CoveredCompetencies = coveredCompetencies,
            MissingCompetencies = missingCompetencies,
            CompetencyCoveragePercent = competencyCoveragePercent
        };
    }

    private class ProjectCompetencyOverview
    {
        public int RequiredCompetencies { get; set; }
        public int CoveredCompetencies { get; set; }
        public int MissingCompetencies { get; set; }
        public decimal CompetencyCoveragePercent { get; set; }
    }
}