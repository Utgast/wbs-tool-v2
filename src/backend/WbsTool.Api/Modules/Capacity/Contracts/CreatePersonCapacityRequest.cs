using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Capacity.Contracts;

/// <summary>
/// Beschreibt die Eingabe fuer verfuegbare Stunden einer Person pro Woche.
/// </summary>
public class CreatePersonCapacityRequest
{
    [Required]
    public Guid PersonId { get; set; }

    [Required]
    public DateOnly WeekStartDate { get; set; }

    [Range(0, 1000)]
    public decimal AvailableHours { get; set; }
}