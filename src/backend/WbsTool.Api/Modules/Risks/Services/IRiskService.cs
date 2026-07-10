using WbsTool.Api.Modules.Risks.Contracts;

namespace WbsTool.Api.Modules.Risks.Services;

public interface IRiskService
{
    IEnumerable<RiskDto> GetByProjectId(Guid projectId);
    RiskDto? GetById(Guid id);
    RiskDto Create(CreateRiskRequest request);
    RiskDto? Update(Guid id, UpdateRiskRequest request);
    RiskDto? Close(Guid id);
    int CountCriticalOpenByProjectId(Guid projectId);
}
