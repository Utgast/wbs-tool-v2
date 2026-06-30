using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Wbs.Contracts;

public class CreateWbsNodeRequest
{
    public Guid? ParentId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    public DateOnly? PlannedStart { get; set; }
    public DateOnly? PlannedEnd { get; set; }
    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }

    public bool IsBlocked { get; set; }
    public string Comment { get; set; } = string.Empty;
}