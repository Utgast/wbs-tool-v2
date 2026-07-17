using WbsTool.Api.Modules.Competencies.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Capacity.Models;

public class PersonAllocation
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? WbsNodeId { get; set; }
    public DateOnly WeekStartDate { get; set; }
    public decimal PlannedHours { get; set; }

    public Person Person { get; set; } = null!;
    public Project Project { get; set; } = null!;
    public WbsNode? WbsNode { get; set; }
}