namespace WbsTool.Api.Modules.Competencies.Models;

public class Competency
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<PersonCompetency> PersonCompetencies { get; set; } = new List<PersonCompetency>();
}