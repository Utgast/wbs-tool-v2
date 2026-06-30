namespace WbsTool.Api.Modules.Wbs.Contracts;

public class ResourceAssignmentDto
{
    public Guid Id { get; set; }

    public Guid WbsNodeId { get; set; }

    public Guid PersonId { get; set; }
    public string PersonDisplayName { get; set; } = string.Empty;
    public bool PersonIsPlaceholder { get; set; }

    public string AssignmentRole { get; set; } = string.Empty;

    public Guid? PlannedRateCategoryId { get; set; }
    public string? PlannedRateCategoryCode { get; set; }

    public Guid? ActualRateCategoryId { get; set; }
    public string? ActualRateCategoryCode { get; set; }

    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }

    public decimal? PlannedCost { get; set; }
    public decimal? ActualCost { get; set; }

    public string Comment { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}