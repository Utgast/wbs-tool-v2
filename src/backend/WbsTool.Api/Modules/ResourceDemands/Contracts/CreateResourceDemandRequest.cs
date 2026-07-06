namespace WbsTool.Api.Modules.ResourceDemands.Contracts;

public class CreateResourceDemandRequest
{
    public Guid ProjectId { get; set; }
    public Guid? WbsNodeId { get; set; }
    public Guid? RequiredCompetencyId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public decimal? PlannedHours { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
}
