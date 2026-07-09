using WbsTool.Api.Modules.Competencies.Contracts;

namespace WbsTool.Api.Modules.Competencies.Services;

public interface ICompetencyService
{
    IEnumerable<CompetencyDto> GetAll();

    IEnumerable<PersonCompetencyDto> GetPersonCompetencies(Guid personId);
    PersonCompetencyDto AddPersonCompetency(Guid personId, AddPersonCompetencyRequest request);

    IEnumerable<WbsRequiredCompetencyDto> GetWbsRequiredCompetencies(Guid wbsNodeId);
    WbsRequiredCompetencyDto AddWbsRequiredCompetency(Guid wbsNodeId, AddWbsRequiredCompetencyRequest request);
    IEnumerable<CompetencyPersonDto> GetPersonsByCompetency(Guid competencyId);
}
