using WbsTool.Api.Modules.Persons.Models;
using WbsTool.Api.Modules.RateCategories.Models;

namespace WbsTool.Api.Modules.Wbs.Models;

public class ResourceAssignment
{
    public Guid Id { get; set; }

    public Guid WbsNodeId { get; set; }
    public WbsNode WbsNode { get; set; } = null!;

    public Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public string AssignmentRole { get; set; } = string.Empty;

    public Guid? PlannedRateCategoryId { get; set; }
    public RateCategory? PlannedRateCategory { get; set; }

    public Guid? ActualRateCategoryId { get; set; }
    public RateCategory? ActualRateCategory { get; set; }

    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }

    public string Comment { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}