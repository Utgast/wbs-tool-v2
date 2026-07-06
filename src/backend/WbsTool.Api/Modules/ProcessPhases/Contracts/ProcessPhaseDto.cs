namespace WbsTool.Api.Modules.ProcessPhases.Contracts;

public class ProcessPhaseDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Goal { get; set; }
    public string? Description { get; set; }
    public string? DefaultResponsibility { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
