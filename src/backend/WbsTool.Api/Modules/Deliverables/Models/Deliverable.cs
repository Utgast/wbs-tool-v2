using WbsTool.Api.Modules.Persons.Models;
using WbsTool.Api.Modules.ProcessPhases.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Deliverables.Models;

public class Deliverable
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DeliverableType Type { get; set; }
    public DeliverableStatus Status { get; set; } = DeliverableStatus.Draft;

    public Guid OwnerPersonId { get; set; }
    public DateOnly DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid ProjectId { get; set; }

    public Guid? ProcessPhaseId { get; set; }
    public Guid? WbsNodeId { get; set; }

    public Project Project { get; set; } = null!;
    public Person OwnerPerson { get; set; } = null!;
    public ProcessPhase? ProcessPhase { get; set; }
    public WbsNode? WbsNode { get; set; }
}
