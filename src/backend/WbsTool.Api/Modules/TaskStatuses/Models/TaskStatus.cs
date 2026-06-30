using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.TaskStatuses.Models;

public class TaskStatus
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Color { get; set; } = "#94A3B8";

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsTerminal { get; set; }

    public ICollection<WbsNode> WbsNodes { get; set; }
        = new List<WbsNode>();
}