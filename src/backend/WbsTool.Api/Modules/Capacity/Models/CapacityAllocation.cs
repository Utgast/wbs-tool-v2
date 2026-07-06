using WbsTool.Api.Modules.Persons.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Capacity.Models;

public class CapacityAllocation
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

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Person Person { get; set; } = null!;
    public Project? Project { get; set; }
    public WbsNode? WbsNode { get; set; }
}
