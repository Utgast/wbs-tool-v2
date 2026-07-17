namespace WbsTool.Api.Modules.Competencies.Models;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<PersonCompetency> PersonCompetencies { get; set; } = new List<PersonCompetency>();
    public ICollection<WbsTool.Api.Modules.Capacity.Models.PersonCapacity> Capacities { get; set; } = new List<WbsTool.Api.Modules.Capacity.Models.PersonCapacity>();
    public ICollection<WbsTool.Api.Modules.Capacity.Models.PersonAllocation> Allocations { get; set; } = new List<WbsTool.Api.Modules.Capacity.Models.PersonAllocation>();
}