using WbsTool.Api.Modules.Capacity.Contracts;

namespace WbsTool.Api.Modules.Capacity.Services;

public interface ICapacityAllocationService
{
    IEnumerable<CapacityAllocationDto> GetAll();
    CapacityAllocationDto Create(CreateCapacityAllocationRequest request);
}
