using WbsTool.Api.Modules.Deliverables.Models;

namespace WbsTool.Api.Modules.Deliverables.Contracts;

public class UpdateDeliverableRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DeliverableType Type { get; set; }
    public DeliverableStatus Status { get; set; }

    public Guid OwnerPersonId { get; set; }
    public DateOnly DueDate { get; set; }

    public Guid? ProcessPhaseId { get; set; }
    public Guid? WbsNodeId { get; set; }
}
