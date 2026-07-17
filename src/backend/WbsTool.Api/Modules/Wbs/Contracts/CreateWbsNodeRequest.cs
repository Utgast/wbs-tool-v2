using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Wbs.Contracts;

public class CreateWbsNodeRequest
{
    public Guid? ParentId { get; set; }

    [Required]
    [MaxLength(100)]
    public string VisibleWbsId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    // Fachlicher Zweck: Status macht den Bearbeitungszustand des Arbeitspakets
    // fuer Projektleitung und Dashboard-Auswertungen direkt sichtbar.
    public string Status { get; set; } = string.Empty;

    [Range(0, 100)]
    public int? ProgressPercent { get; set; }

    [MaxLength(200)]
    public string ResponsiblePersonName { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    public int Level { get; set; }
    public int SortOrder { get; set; }

    public DateOnly? PlannedStart { get; set; }
    public DateOnly? PlannedEnd { get; set; }
    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }

    public bool IsBlocked { get; set; }
    public string Comment { get; set; } = string.Empty;
}