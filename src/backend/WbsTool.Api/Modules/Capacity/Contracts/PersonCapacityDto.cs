namespace WbsTool.Api.Modules.Capacity.Contracts;

/// <summary>
/// Uebertraegt die verfuegbare Wochenkapazitaet einer Person.
/// </summary>
public class PersonCapacityDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public DateOnly WeekStartDate { get; set; }
    public decimal AvailableHours { get; set; }
}