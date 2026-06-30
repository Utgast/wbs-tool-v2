using WbsTool.Api.Modules.TaskStatuses.Contracts;

namespace WbsTool.Api.Modules.TaskStatuses.Services;

public interface ITaskStatusService
{
    IEnumerable<TaskStatusDto> GetAllActive();
}