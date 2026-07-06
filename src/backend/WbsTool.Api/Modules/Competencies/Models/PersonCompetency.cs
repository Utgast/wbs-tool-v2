using WbsTool.Api.Modules.Persons.Models;

namespace WbsTool.Api.Modules.Competencies.Models;

public class PersonCompetency
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }
    public Guid CompetencyId { get; set; }

    public int? ProficiencyLevel { get; set; }
    public string? Comment { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Person Person { get; set; } = null!;
    public Competency Competency { get; set; } = null!;
}
