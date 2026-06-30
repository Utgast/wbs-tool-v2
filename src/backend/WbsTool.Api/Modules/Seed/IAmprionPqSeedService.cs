namespace WbsTool.Api.Modules.Seed.Services;

public interface IAmprionPqSeedService
{
    Task<AmprionPqSeedResultDto> SeedAsync(CancellationToken cancellationToken = default);
}

public class AmprionPqSeedResultDto
{
    public Guid ProjectId { get; set; }
    public string ProjectNumber { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;

    public int PersonsUpserted { get; set; }
    public int RateCategoriesUpserted { get; set; }
    public int TaskStatusesUpserted { get; set; }
    public int WbsNodesCreated { get; set; }
    public int ResourceAssignmentsCreated { get; set; }

    public string Message { get; set; } = string.Empty;
}