namespace WbsTool.Api.Modules.Competencies.Contracts;

public class CompetencyDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int AssignedPersonsCount { get; set; }
}
