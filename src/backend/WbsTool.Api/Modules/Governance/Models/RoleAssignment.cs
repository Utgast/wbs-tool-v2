using WbsTool.Api.Modules.Persons.Models;

namespace WbsTool.Api.Modules.Governance.Models;

public class RoleAssignment
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }
    public AppRole Role { get; set; }
    public ScopeType ScopeType { get; set; }
    public Guid? ProjectId { get; set; }

    public string AssignedBy { get; set; } = string.Empty;
    public DateTime AssignedAtUtc { get; set; }

    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidUntil { get; set; }

    public string? Comment { get; set; }
    public bool IsActive { get; set; } = true;

    public string? RevokedBy { get; set; }
    public DateTime? RevokedAtUtc { get; set; }

    public Person Person { get; set; } = null!;
}
