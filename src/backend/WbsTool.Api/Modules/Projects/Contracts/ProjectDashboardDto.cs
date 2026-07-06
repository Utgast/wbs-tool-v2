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

    public decimal PlannedDemandHours { get; set; }
    public decimal AssignedHours { get; set; }
    public decimal OpenHours { get; set; }
    public decimal CapacityHours { get; set; }
    public decimal UtilizationPercent { get; set; }
}