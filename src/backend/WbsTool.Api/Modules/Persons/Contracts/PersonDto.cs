namespace WbsTool.Api.Modules.Persons.Contracts;

public class PersonDto
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? Email { get; set; }

    public bool IsPlaceholder { get; set; }
    public string? PlaceholderType { get; set; }

    public bool IsActive { get; set; }
}