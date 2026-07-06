using WbsTool.Api.Modules.ProcessPhases.Contracts;

namespace WbsTool.Api.Modules.ProcessPhases.Services;

public interface IProcessPhaseService
{
    IEnumerable<ProcessPhaseDto> GetAll();
    IEnumerable<ProcessPhaseDto> GetByProjectId(Guid projectId);
}
