using WbsTool.Api.Modules.Projects.Contracts;

namespace WbsTool.Api.Modules.Projects.Services;

public interface IManagementAttentionService
{
    List<ManagementAttentionViewDto> GetAttentionItems(Guid projectId);
}
