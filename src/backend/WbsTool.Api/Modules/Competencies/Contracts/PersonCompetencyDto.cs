namespace WbsTool.Api.Modules.Competencies.Contracts;

/// <summary>
/// Uebertraegt eine reine Zuordnung zwischen Person und Kompetenz.
/// </summary>
public class PersonCompetencyDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public Guid CompetencyId { get; set; }
    public string CompetencyName { get; set; } = string.Empty;
}