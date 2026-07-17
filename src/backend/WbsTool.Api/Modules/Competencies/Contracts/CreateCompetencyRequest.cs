using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Competencies.Contracts;

/// <summary>
/// Beschreibt die minimalen Eingabedaten zum Anlegen einer Kompetenz.
/// </summary>
public class CreateCompetencyRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}