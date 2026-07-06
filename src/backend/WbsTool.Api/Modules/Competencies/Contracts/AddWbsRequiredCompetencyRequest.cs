namespace WbsTool.Api.Modules.Competencies.Contracts;

public class AddWbsRequiredCompetencyRequest
{
    public Guid ProjectId { get; set; }
    public Guid CompetencyId { get; set; }

    public int? RequiredLevel { get; set; }
    public string? Comment { get; set; }
}
