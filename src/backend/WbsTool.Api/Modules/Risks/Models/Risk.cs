using WbsTool.Api.Modules.Persons.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Risks.Models;

public class Risk
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public RiskCategory Category { get; set; }
    public RiskSeverity Severity { get; set; }
    public RiskStatus Status { get; set; } = RiskStatus.New;

    public Guid OwnerPersonId { get; set; }
    public Guid ProjectId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateOnly? DueDate { get; set; }
    public Guid? WbsNodeId { get; set; }

    public Person OwnerPerson { get; set; } = null!;
    public Project Project { get; set; } = null!;
    public WbsNode? WbsNode { get; set; }
}
