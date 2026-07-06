using WbsTool.Api.Modules.ResourceDemands.Models;

namespace WbsTool.Api.Modules.ResourceDemands.Contracts;

public class ResourceDemandDto
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Guid? WbsNodeId { get; set; }
    public Guid? RequiredCompetencyId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public decimal? PlannedHours { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public ResourceDemandStatus Status { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public string? DecisionComment { get; set; }
}
