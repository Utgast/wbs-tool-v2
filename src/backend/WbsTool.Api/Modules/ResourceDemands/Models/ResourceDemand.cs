using WbsTool.Api.Modules.Competencies.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.ResourceDemands.Models;

public class ResourceDemand
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

    public ResourceDemandStatus Status { get; set; } = ResourceDemandStatus.Draft;

    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public string? StatusChangedBy { get; set; }
    public DateTime? StatusChangedAtUtc { get; set; }

    public string? DecisionComment { get; set; }

    public Project Project { get; set; } = null!;
    public WbsNode? WbsNode { get; set; }
    public Competency? RequiredCompetency { get; set; }
}
