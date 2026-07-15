using WbsTool.Api.Modules.Wbs.Contracts;

namespace WbsTool.Api.Modules.Wbs.Services;

public interface IWbsService
{
    IEnumerable<WbsNodeDto> GetByProjectId(Guid projectId);
    IEnumerable<WbsTreeNodeDto> GetTreeByProjectId(Guid projectId);
    WbsNodeDto Create(Guid projectId, CreateWbsNodeRequest request);
}