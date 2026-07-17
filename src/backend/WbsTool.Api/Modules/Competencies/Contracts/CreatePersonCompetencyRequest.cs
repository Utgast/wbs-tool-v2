using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Competencies.Contracts;

/// <summary>
/// Beschreibt eine neue, belegbare Zuordnung Person zu Kompetenz.
/// </summary>
public class CreatePersonCompetencyRequest
{
    [Required]
    public Guid PersonId { get; set; }

    [Required]
    public Guid CompetencyId { get; set; }
}