using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.TaskStatuses.Contracts;
using WbsTool.Api.Modules.TaskStatuses.Services;

namespace WbsTool.Api.Modules.TaskStatuses.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskStatusesController : ControllerBase
{
    private readonly ITaskStatusService _taskStatusService;

    public TaskStatusesController(ITaskStatusService taskStatusService)
    {
        _taskStatusService = taskStatusService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TaskStatusDto>> GetAllActive()
    {
        var taskStatuses = _taskStatusService.GetAllActive();
        return Ok(taskStatuses);
    }
}