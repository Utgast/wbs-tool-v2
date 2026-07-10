using WbsTool.Api.Modules.Deliverables.Contracts;

namespace WbsTool.Api.Modules.Deliverables.Services;

public interface IDeliverableService
{
    IEnumerable<DeliverableDto> GetByProjectId(Guid projectId);
    DeliverableDto? GetById(Guid id);
    DeliverableDto Create(CreateDeliverableRequest request);
    DeliverableDto? Update(Guid id, UpdateDeliverableRequest request);
    DeliverableDto? MarkDelivered(Guid id);
    int CountOverdueByProjectId(Guid projectId);
}
