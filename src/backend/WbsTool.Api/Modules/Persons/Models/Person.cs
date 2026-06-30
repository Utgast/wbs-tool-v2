using WbsTool.Api.Modules.Wbs.Models;

namespace WbsTool.Api.Modules.Persons.Models;

public class Person
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? Email { get; set; }

    public bool IsPlaceholder { get; set; }
    public string? PlaceholderType { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<ResourceAssignment> ResourceAssignments { get; set; }
        = new List<ResourceAssignment>();
}