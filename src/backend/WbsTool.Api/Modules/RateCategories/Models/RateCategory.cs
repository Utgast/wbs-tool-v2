using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.RateCategories.Models;

public class RateCategory
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public decimal HourlyRate { get; set; }
    public string Currency { get; set; } = "EUR";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<ResourceAssignment> PlannedAssignments { get; set; }
        = new List<ResourceAssignment>();

    public ICollection<ResourceAssignment> ActualAssignments { get; set; }
        = new List<ResourceAssignment>();
}