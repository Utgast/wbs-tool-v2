namespace WbsTool.Api.Modules.Capacity.Contracts;

public class CapacityAllocationDto
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? WbsNodeId { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public decimal? AllocationPercent { get; set; }

    public string Source { get; set; } = string.Empty;
    public string? Comment { get; set; }

    public bool IsActive { get; set; }
}
