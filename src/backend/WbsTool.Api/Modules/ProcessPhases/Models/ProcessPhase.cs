namespace WbsTool.Api.Modules.ProcessPhases.Models;

public class ProcessPhase
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Goal { get; set; }
    public string? Description { get; set; }
    public string? DefaultResponsibility { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
