using WbsTool.Api.Modules.ResourceDemands.Models;

namespace WbsTool.Api.Modules.ResourceDemands.Contracts;

public class UpdateResourceDemandRequest
{
    public Guid? WbsNodeId { get; set; }
    public Guid? RequiredCompetencyId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public decimal? PlannedHours { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public ResourceDemandStatus Status { get; set; }

    public string UpdatedBy { get; set; } = string.Empty;
    public string? DecisionComment { get; set; }
}
