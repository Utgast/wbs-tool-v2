namespace WbsTool.Api.Modules.Competencies.Contracts;

public class AddPersonCompetencyRequest
{
    public Guid CompetencyId { get; set; }

    public int? ProficiencyLevel { get; set; }
    public string? Comment { get; set; }
}
