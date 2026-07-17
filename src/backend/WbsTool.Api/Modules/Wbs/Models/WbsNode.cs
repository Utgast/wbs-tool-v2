using WbsTool.Api.Modules.Projects.Models;

namespace WbsTool.Api.Modules.Wbs.Models;

public class WbsNode
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? ParentId { get; set; }

    public string VisibleWbsId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public WbsNodeStatus Status { get; set; } = WbsNodeStatus.Offen;
    public int ProgressPercent { get; set; }
    public string ResponsiblePersonName { get; set; } = string.Empty;

    public WbsNodeType Type { get; set; }
    public int Level { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public DateOnly? PlannedStart { get; set; }
    public DateOnly? PlannedEnd { get; set; }
    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }

    public bool IsBlocked { get; set; }
    public string Comment { get; set; } = string.Empty;

    public Project Project { get; set; } = null!;
    public WbsNode? Parent { get; set; }
    public ICollection<WbsNode> Children { get; set; } = new List<WbsNode>();
}