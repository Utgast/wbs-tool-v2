namespace WbsTool.Api.Modules.Competencies.Contracts;

public class CompetencyPersonDto
{
    public Guid PersonCompetencyId { get; set; }

    public Guid PersonId { get; set; }
    public string PersonDisplayName { get; set; } = string.Empty;
    public string? PersonShortName { get; set; }
    public string? PersonEmail { get; set; }

    public Guid CompetencyId { get; set; }
    public string CompetencyCode { get; set; } = string.Empty;
    public string CompetencyName { get; set; } = string.Empty;

    public int? ProficiencyLevel { get; set; }
    public string? Comment { get; set; }
    public bool IsActive { get; set; }
}
