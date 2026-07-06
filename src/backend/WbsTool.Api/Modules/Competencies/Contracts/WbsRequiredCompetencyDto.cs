namespace WbsTool.Api.Modules.Competencies.Contracts;

public class WbsRequiredCompetencyDto
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Guid WbsNodeId { get; set; }
    public Guid CompetencyId { get; set; }
    public string CompetencyCode { get; set; } = string.Empty;
    public string CompetencyName { get; set; } = string.Empty;

    public int? RequiredLevel { get; set; }
    public string? Comment { get; set; }
}
