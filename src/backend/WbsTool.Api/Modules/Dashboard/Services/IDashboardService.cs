using WbsTool.Api.Modules.Dashboard.Contracts;

namespace WbsTool.Api.Modules.Dashboard.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}
