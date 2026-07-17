using WbsTool.Api.Modules.Capacity.Contracts;
using WbsTool.Api.Modules.Competencies.Contracts;

namespace WbsTool.Api.Modules.Capacity.Services;

public interface ICapacityService
{
    Task<IEnumerable<PersonDto>> GetPersonsAsync();
    Task<PersonDto> CreatePersonAsync(CreatePersonRequest request);
    Task<bool> DeletePersonAsync(Guid id);
    Task<IEnumerable<PersonCapacityDto>> GetCapacitiesAsync();
    Task<PersonCapacityDto> CreateCapacityAsync(CreatePersonCapacityRequest request);
    Task<bool> DeleteCapacityAsync(Guid id);
    Task<IEnumerable<PersonAllocationDto>> GetAllocationsAsync();
    Task<PersonAllocationDto> CreateAllocationAsync(CreatePersonAllocationRequest request);
    Task<bool> DeleteAllocationAsync(Guid id);
}