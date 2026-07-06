using WbsTool.Api.Modules.ProcessPhases.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.ProcessPhases.Models;

public class WbsPhaseMapping
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Guid WbsNodeId { get; set; }
    public Guid ProcessPhaseId { get; set; }

    public bool IsPrimary { get; set; }
    public string? Comment { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public WbsNode WbsNode { get; set; } = null!;
    public ProcessPhase ProcessPhase { get; set; } = null!;
}
