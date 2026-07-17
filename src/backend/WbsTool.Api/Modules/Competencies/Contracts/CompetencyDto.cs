namespace WbsTool.Api.Modules.Competencies.Contracts;

/// <summary>
/// Uebertraegt eine bearbeitbare Kompetenz ohne Bewertungslogik.
/// </summary>
public class CompetencyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}