using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Capacity.Contracts;

/// <summary>
/// Beschreibt eine geplante Stundenallokation fuer Projekt oder Arbeitspaket.
/// </summary>
public class CreatePersonAllocationRequest
{
    [Required]
    public Guid PersonId { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    public Guid? WbsNodeId { get; set; }

    [Required]
    public DateOnly WeekStartDate { get; set; }

    [Range(0, 1000)]
    public decimal PlannedHours { get; set; }
}