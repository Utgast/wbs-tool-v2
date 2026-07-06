using WbsTool.Api.Modules.ResourceDemands.Contracts;

namespace WbsTool.Api.Modules.ResourceDemands.Services;

public interface IResourceDemandService
{
    IEnumerable<ResourceDemandDto> GetAll();
    ResourceDemandDto? GetById(Guid id);
    ResourceDemandDto Create(CreateResourceDemandRequest request);
    ResourceDemandDto? Update(Guid id, UpdateResourceDemandRequest request);
}
