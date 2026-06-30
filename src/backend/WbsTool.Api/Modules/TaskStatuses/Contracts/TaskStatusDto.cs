namespace WbsTool.Api.Modules.TaskStatuses.Contracts;

public class TaskStatusDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public bool IsTerminal { get; set; }
}