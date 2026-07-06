using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Competencies.Models;

public class WbsRequiredCompetency
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Guid WbsNodeId { get; set; }
    public Guid CompetencyId { get; set; }

    public int? RequiredLevel { get; set; }
    public string? Comment { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public WbsNode WbsNode { get; set; } = null!;
    public Competency Competency { get; set; } = null!;
}
