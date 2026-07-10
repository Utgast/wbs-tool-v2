using WbsTool.Api.Modules.Risks.Models;

namespace WbsTool.Api.Modules.Risks.Contracts;

public class UpdateRiskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public RiskCategory Category { get; set; }
    public RiskSeverity Severity { get; set; }
    public RiskStatus Status { get; set; }

    public Guid OwnerPersonId { get; set; }

    public DateOnly? DueDate { get; set; }
    public Guid? WbsNodeId { get; set; }
}
