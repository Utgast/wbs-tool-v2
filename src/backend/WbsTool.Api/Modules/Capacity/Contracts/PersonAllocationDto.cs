namespace WbsTool.Api.Modules.Capacity.Contracts;

/// <summary>
/// Uebertraegt eine geplante Stundenallokation einer Person.
/// </summary>
public class PersonAllocationDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public Guid? WbsNodeId { get; set; }
    public DateOnly WeekStartDate { get; set; }
    public decimal PlannedHours { get; set; }
}