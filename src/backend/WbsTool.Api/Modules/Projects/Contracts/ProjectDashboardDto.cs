namespace WbsTool.Api.Modules.Projects.Contracts;

public class ProjectDashboardDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public decimal TotalPlannedHours { get; set; }
    public decimal TotalActualHours { get; set; }
    public decimal TotalPlannedCost { get; set; }
    public decimal TotalActualCost { get; set; }

    public decimal ProgressPercent { get; set; }

    public int BlockedNodes { get; set; }
    public int OverdueNodes { get; set; }

    public int OpenRisks { get; set; }
    public int CriticalRisks { get; set; }
    public int OpenDeliverables { get; set; }
    public int OverdueDeliverables { get; set; }
    public string DeliveryStatus { get; set; } = "Green";
    public string DeliveryStatusReason { get; set; } = string.Empty;
    public string DeliveryStatusTrigger { get; set; } = string.Empty;

    public List<ProjectDashboardRiskItemDto> OpenRiskItems { get; set; } = [];
    public List<ProjectDashboardRiskItemDto> CriticalRiskItems { get; set; } = [];
    public List<ProjectDashboardDeliverableItemDto> OpenDeliverableItems { get; set; } = [];
    public List<ProjectDashboardDeliverableItemDto> OverdueDeliverableItems { get; set; } = [];

    public decimal PlannedDemandHours { get; set; }
    public decimal AssignedHours { get; set; }
    public decimal CoveredHours { get; set; }
    public decimal OpenHours { get; set; }
    public decimal CapacityHours { get; set; }
    public decimal UtilizationPercent { get; set; }

    public List<ProjectDashboardRiskItemDto> TopRiskItems { get; set; } = [];
    public List<ProjectDashboardDeliverableItemDto> CriticalDeliverableItems { get; set; } = [];

    public List<ManagementAttentionViewDto> TopManagementAttentionItems { get; set; } = [];

public int RequiredCompetencies { get; set; }
public int CoveredCompetencies { get; set; }
public int MissingCompetencies { get; set; }
public decimal CompetencyCoveragePercent { get; set; }
}

public class ProjectDashboardRiskItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid OwnerPersonId { get; set; }
}

public class ProjectDashboardDeliverableItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
}
