using WbsTool.Api.Modules.Wbs.Contracts;

namespace WbsTool.Api.Modules.Wbs.Services;

public interface IResourceAssignmentService
{
    IEnumerable<ResourceAssignmentDto> GetByWbsNode(Guid projectId, Guid wbsNodeId);

    ResourceAssignmentDto Create(
        Guid projectId,
        Guid wbsNodeId,
        CreateResourceAssignmentRequest request);

    ResourceAssignmentDto? Update(
        Guid projectId,
        Guid wbsNodeId,
        Guid assignmentId,
        UpdateResourceAssignmentRequest request);

    bool Deactivate(
        Guid projectId,
        Guid wbsNodeId,
        Guid assignmentId);
}