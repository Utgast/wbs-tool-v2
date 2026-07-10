using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Deliverables.Models;
using WbsTool.Api.Modules.Projects.Contracts;
using WbsTool.Api.Modules.Risks.Models;

namespace WbsTool.Api.Modules.Projects.Services;

public class ProjectDashboardService : IProjectDashboardService
{
    private readonly AppDbContext _dbContext;
    private readonly IManagementAttentionService _managementAttentionService;

    public ProjectDashboardService(
        AppDbContext dbContext,
        IManagementAttentionService managementAttentionService)
    {
        _dbContext = dbContext;
        _managementAttentionService = managementAttentionService;
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

        var openRisks = _dbContext.Risks
            .AsNoTracking()
            .Count(r =>
                r.ProjectId == projectId &&
                r.Status != RiskStatus.Accepted &&
                r.Status != RiskStatus.Closed);

        var criticalRisks = _dbContext.Risks
            .AsNoTracking()
            .Count(r =>
                r.ProjectId == projectId &&
                r.Severity == RiskSeverity.High &&
                r.Status != RiskStatus.Accepted &&
                r.Status != RiskStatus.Closed);

        var openDeliverables = _dbContext.Deliverables
            .AsNoTracking()
            .Count(d =>
                d.ProjectId == projectId &&
                d.Status != DeliverableStatus.Delivered);

        var overdueDeliverables = _dbContext.Deliverables
            .AsNoTracking()
            .Count(d =>
                d.ProjectId == projectId &&
                d.DueDate < today &&
                d.Status != DeliverableStatus.Delivered);

        var openRiskItems = _dbContext.Risks
            .AsNoTracking()
            .Where(r =>
                r.ProjectId == projectId &&
                r.Status != RiskStatus.Accepted &&
                r.Status != RiskStatus.Closed)
            .OrderByDescending(r => r.CreatedAt)
            .ToList()
            .Select(r => new ProjectDashboardRiskItemDto
            {
                Title = r.Title,
                Severity = r.Severity.ToString(),
                Status = r.Status.ToString(),
                OwnerPersonId = r.OwnerPersonId
            })
            .ToList();

        var criticalRiskItems = _dbContext.Risks
            .AsNoTracking()
            .Where(r =>
                r.ProjectId == projectId &&
                r.Severity == RiskSeverity.High &&
                r.Status != RiskStatus.Accepted &&
                r.Status != RiskStatus.Closed)
            .OrderByDescending(r => r.CreatedAt)
            .ToList()
            .Select(r => new ProjectDashboardRiskItemDto
            {
                Title = r.Title,
                Severity = r.Severity.ToString(),
                Status = r.Status.ToString(),
                OwnerPersonId = r.OwnerPersonId
            })
            .ToList();

        var openDeliverableItems = _dbContext.Deliverables
            .AsNoTracking()
            .Where(d =>
                d.ProjectId == projectId &&
                d.Status != DeliverableStatus.Delivered)
            .OrderBy(d => d.DueDate)
            .ToList()
            .Select(d => new ProjectDashboardDeliverableItemDto
            {
                Name = d.Name,
                Type = d.Type.ToString(),
                Status = d.Status.ToString(),
                DueDate = d.DueDate
            })
            .ToList();

        var overdueDeliverableItems = _dbContext.Deliverables
            .AsNoTracking()
            .Where(d =>
                d.ProjectId == projectId &&
                d.DueDate < today &&
                d.Status != DeliverableStatus.Delivered)
            .OrderBy(d => d.DueDate)
            .ToList()
            .Select(d => new ProjectDashboardDeliverableItemDto
            {
                Name = d.Name,
                Type = d.Type.ToString(),
                Status = d.Status.ToString(),
                DueDate = d.DueDate
            })
            .ToList();

        var topRiskItems = _dbContext.Risks
            .AsNoTracking()
            .Where(r =>
                r.ProjectId == projectId &&
                r.Status != RiskStatus.Accepted &&
                r.Status != RiskStatus.Closed)
            .ToList()
            .OrderBy(r => RiskSeverityRank(r.Severity))
            .ThenByDescending(r => r.CreatedAt)
            .Take(5)
            .Select(r => new ProjectDashboardRiskItemDto
            {
                Title = r.Title,
                Severity = r.Severity.ToString(),
                Status = r.Status.ToString(),
                OwnerPersonId = r.OwnerPersonId
            })
            .ToList();

        var criticalDeliverableItems = _dbContext.Deliverables
            .AsNoTracking()
            .Where(d =>
                d.ProjectId == projectId &&
                d.Status != DeliverableStatus.Delivered)
            .ToList()
            .OrderBy(d => DeliverablePriorityRank(d, today))
            .ThenBy(d => d.DueDate)
            .Take(5)
            .Select(d => new ProjectDashboardDeliverableItemDto
            {
                Name = d.Name,
                Type = d.Type.ToString(),
                Status = d.Status.ToString(),
                DueDate = d.DueDate
            })
            .ToList();

        var topManagementAttentionItems = _managementAttentionService
            .GetAttentionItems(projectId)
            .Take(10)
            .ToList();

        var resourceOverview = GetResourceOverview(projectId);
        var competencyOverview = GetCompetencyOverview(projectId);

        var competencyCoveragePercent = competencyOverview.CompetencyCoveragePercent;
        var utilizationPercent = resourceOverview?.UtilizationPercent ?? 0m;
        var openHours = resourceOverview?.OpenHours ?? 0m;
        var coveredHours = resourceOverview?.AssignedHours ?? 0m;

        var deliveryStatus = "Green";
        var deliveryStatusReason = "Keine RED- oder YELLOW-Regel aktiv";
        var deliveryStatusTrigger = "Kein Ausloeser";

        if (criticalRisks > 0)
        {
            deliveryStatus = "Red";
            deliveryStatusReason = "Kritische Risiken vorhanden";
            deliveryStatusTrigger = "CriticalRisks";
        }
        else if (overdueDeliverables > 0)
        {
            deliveryStatus = "Red";
            deliveryStatusReason = "Ueberfaellige Deliverables vorhanden";
            deliveryStatusTrigger = "OverdueDeliverables";
        }
        else if (competencyCoveragePercent < 50m)
        {
            deliveryStatus = "Red";
            deliveryStatusReason = "Kompetenzdeckung unter 50 Prozent";
            deliveryStatusTrigger = "CompetencyCoveragePercent < 50";
        }
        else if (openRisks > 0)
        {
            deliveryStatus = "Yellow";
            deliveryStatusReason = "Offene Risiken vorhanden";
            deliveryStatusTrigger = "OpenRisks";
        }
        else if (openDeliverables > 0)
        {
            deliveryStatus = "Yellow";
            deliveryStatusReason = "Offene Deliverables vorhanden";
            deliveryStatusTrigger = "OpenDeliverables";
        }
        else if (competencyCoveragePercent < 80m)
        {
            deliveryStatus = "Yellow";
            deliveryStatusReason = "Kompetenzdeckung unter 80 Prozent";
            deliveryStatusTrigger = "CompetencyCoveragePercent < 80";
        }
        else if (utilizationPercent > 100m)
        {
            deliveryStatus = "Yellow";
            deliveryStatusReason = "Auslastung ueber 100 Prozent";
            deliveryStatusTrigger =
                $"UtilizationPercent > 100 (OpenHours={openHours}, CoveredHours={coveredHours})";
        }

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

            OpenRisks = openRisks,
            CriticalRisks = criticalRisks,
            OpenDeliverables = openDeliverables,
            OverdueDeliverables = overdueDeliverables,
            DeliveryStatus = deliveryStatus,
            DeliveryStatusReason = deliveryStatusReason,
            DeliveryStatusTrigger = deliveryStatusTrigger,
            OpenRiskItems = openRiskItems,
            CriticalRiskItems = criticalRiskItems,
            OpenDeliverableItems = openDeliverableItems,
            OverdueDeliverableItems = overdueDeliverableItems,

            PlannedDemandHours = resourceOverview?.PlannedDemandHours ?? 0m,
            AssignedHours = resourceOverview?.AssignedHours ?? 0m,
            CoveredHours = resourceOverview?.AssignedHours ?? 0m,
            OpenHours = resourceOverview?.OpenHours ?? 0m,

            CapacityHours = resourceOverview?.CapacityHours ?? 0m,
            UtilizationPercent = utilizationPercent,

            RequiredCompetencies = competencyOverview.RequiredCompetencies,
            CoveredCompetencies = competencyOverview.CoveredCompetencies,
            MissingCompetencies = competencyOverview.MissingCompetencies,
            CompetencyCoveragePercent = competencyCoveragePercent,
            TopRiskItems = topRiskItems,
            CriticalDeliverableItems = criticalDeliverableItems,
            TopManagementAttentionItems = topManagementAttentionItems
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

    private static int RiskSeverityRank(RiskSeverity severity)
    {
        return severity switch
        {
            RiskSeverity.High => 1,
            RiskSeverity.Medium => 2,
            _ => 3
        };
    }

    private static int DeliverablePriorityRank(Deliverable deliverable, DateOnly today)
    {
        if (deliverable.DueDate < today)
        {
            return 1;
        }

        if (deliverable.Status == DeliverableStatus.Review)
        {
            return 2;
        }

        return 3;
    }
}