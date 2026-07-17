using WbsTool.Api.Modules.Competencies.Models;

namespace WbsTool.Api.Modules.Capacity.Models;

public class PersonCapacity
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public DateOnly WeekStartDate { get; set; }
    public decimal AvailableHours { get; set; }

    public Person Person { get; set; } = null!;
}