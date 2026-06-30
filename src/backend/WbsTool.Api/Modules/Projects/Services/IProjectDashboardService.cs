using WbsTool.Api.Modules.Projects.Contracts;

namespace WbsTool.Api.Modules.Projects.Services;

public interface IProjectDashboardService
{
    ProjectDashboardDto? GetDashboard(Guid projectId);
}