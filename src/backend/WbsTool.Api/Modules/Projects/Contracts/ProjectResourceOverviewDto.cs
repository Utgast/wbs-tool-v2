namespace WbsTool.Api.Modules.Projects.Contracts;

public class ProjectResourceOverviewDto
{
    public decimal PlannedDemandHours { get; set; }

    public decimal AssignedHours { get; set; }

    public decimal OpenHours { get; set; }

    public decimal CapacityHours { get; set; }

    public decimal UtilizationPercent { get; set; }
}
