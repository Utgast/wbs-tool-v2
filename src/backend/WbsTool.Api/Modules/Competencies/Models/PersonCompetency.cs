namespace WbsTool.Api.Modules.Competencies.Models;

public class PersonCompetency
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public Guid CompetencyId { get; set; }

    public Person Person { get; set; } = null!;
    public Competency Competency { get; set; } = null!;
}