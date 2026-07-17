using WbsTool.Api.Modules.Competencies.Contracts;

namespace WbsTool.Api.Modules.Competencies.Services;

public interface ICompetencyService
{
    Task<IEnumerable<CompetencyDto>> GetCompetenciesAsync();
    Task<CompetencyDto> CreateCompetencyAsync(CreateCompetencyRequest request);
    Task<bool> DeleteCompetencyAsync(Guid id);
    Task<IEnumerable<PersonCompetencyDto>> GetPersonCompetenciesAsync();
    Task<PersonCompetencyDto> CreatePersonCompetencyAsync(CreatePersonCompetencyRequest request);
    Task<bool> DeletePersonCompetencyAsync(Guid id);
}