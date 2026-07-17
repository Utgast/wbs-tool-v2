namespace WbsTool.Api.Modules.Dashboard.Contracts;

public class DashboardDto
{
    public int ProjectsCount { get; set; }
    public int WorkPackagesCount { get; set; }
    public int OpenWorkPackagesCount { get; set; }
    public int BlockedWorkPackagesCount { get; set; }
    public decimal ProjectProgressPercent { get; set; }
    public int ActionRequiredCount { get; set; }
}
