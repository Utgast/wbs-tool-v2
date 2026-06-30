using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Wbs.Contracts;

public class CreateResourceAssignmentRequest
{
    [Required]
    public Guid PersonId { get; set; }

    [Required]
    [MaxLength(50)]
    public string AssignmentRole { get; set; } = string.Empty;

    public Guid? PlannedRateCategoryId { get; set; }
    public Guid? ActualRateCategoryId { get; set; }

    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }

    [MaxLength(2000)]
    public string Comment { get; set; } = string.Empty;
}