using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Persons.Models;
using WbsTool.Api.Modules.Projects.Models;
using WbsTool.Api.Modules.RateCategories.Models;
using WbsTool.Api.Modules.Wbs.Models;
using WbsTaskStatus = WbsTool.Api.Modules.TaskStatuses.Models.TaskStatus;

namespace WbsTool.Api.Modules.Seed.Services;

public class AmprionPqSeedService : IAmprionPqSeedService
{
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;

    public AmprionPqSeedService(
        AppDbContext dbContext,
        IWebHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    public async Task<AmprionPqSeedResultDto> SeedAsync(CancellationToken cancellationToken = default)
    {
        var seedFilePath = Path.Combine(
            _environment.ContentRootPath,
            "Data",
            "Seed",
            "amprion_pq_seed_data.json");

        if (!File.Exists(seedFilePath))
        {
            throw new FileNotFoundException(
                $"Seed-Datei wurde nicht gefunden: {seedFilePath}");
        }

        var json = await File.ReadAllTextAsync(seedFilePath, cancellationToken);

        var seedData = JsonSerializer.Deserialize<AmprionPqSeedData>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (seedData is null)
        {
            throw new InvalidOperationException("Seed-Datei konnte nicht gelesen werden.");
        }

        var now = DateTime.UtcNow;

        await UpsertTaskStatuses(seedData.TaskStatuses, cancellationToken);
        await UpsertRateCategories(seedData.RateCategories, now, cancellationToken);
        await UpsertPersons(seedData.Persons, now, cancellationToken);

        var project = await UpsertProject(seedData.Project, cancellationToken);

        await RemoveExistingProjectWbs(project.Id, cancellationToken);

        var statusByCode = await _dbContext.TaskStatuses
            .AsNoTracking()
            .ToDictionaryAsync(s => s.Code, s => s.Id, cancellationToken);

        var personByName = await _dbContext.Persons
            .AsNoTracking()
            .ToDictionaryAsync(p => p.DisplayName, p => p.Id, cancellationToken);

        var rateCategoryByCode = await _dbContext.RateCategories
            .AsNoTracking()
            .ToDictionaryAsync(r => r.Code, r => r.Id, cancellationToken);

        var nodeByVisibleWbsId = new Dictionary<string, WbsNode>();

        var orderedNodes = seedData.WbsNodes
            .OrderBy(n => n.Level ?? CalculateLevel(n.VisibleWbsId))
            .ThenBy(n => GetSortOrderFromVisibleWbsId(n.VisibleWbsId))
            .ToList();

        foreach (var seedNode in orderedNodes)
        {
            var parentId = ResolveParentId(seedNode, nodeByVisibleWbsId);

            var statusCode = string.IsNullOrWhiteSpace(seedNode.StatusCode)
                ? "Empty"
                : seedNode.StatusCode;

            var statusId = statusByCode.TryGetValue(statusCode, out var foundStatusId)
                ? foundStatusId
                : statusByCode["Empty"];

            var node = new WbsNode
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                ParentId = parentId,
                VisibleWbsId = seedNode.VisibleWbsId,
                Title = seedNode.Title,
                Description = string.Empty,
                Type = ParseWbsNodeType(seedNode.Type),
                Level = seedNode.Level ?? CalculateLevel(seedNode.VisibleWbsId),
                SortOrder = GetSortOrderFromVisibleWbsId(seedNode.VisibleWbsId),
                IsActive = seedNode.IsActive,
                StatusId = statusId,
                PlannedStart = ParseDate(seedNode.PlannedStart),
                PlannedEnd = ParseDate(seedNode.PlannedEnd),
                PlannedHours = seedNode.PlannedHoursLegacy,
                ActualHours = seedNode.ActualHoursLegacy,
                ImportedPlannedCost = null,
                ImportedActualCost = null,
                IsBlocked = seedNode.IsBlocked || statusCode == "Critical",
                Comment = seedNode.Comment ?? string.Empty
            };

            _dbContext.WbsNodes.Add(node);
            nodeByVisibleWbsId[node.VisibleWbsId] = node;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var assignmentsCreated = 0;

        foreach (var seedAssignment in seedData.ResourceAssignments)
        {
            if (!nodeByVisibleWbsId.TryGetValue(seedAssignment.VisibleWbsId, out var node))
            {
                continue;
            }

            if (!personByName.TryGetValue(seedAssignment.PersonDisplayName, out var personId))
            {
                continue;
            }

            Guid? plannedRateCategoryId = null;
            if (!string.IsNullOrWhiteSpace(seedAssignment.PlannedRateCategoryCode) &&
                rateCategoryByCode.TryGetValue(seedAssignment.PlannedRateCategoryCode, out var foundPlannedRateCategoryId))
            {
                plannedRateCategoryId = foundPlannedRateCategoryId;
            }

            Guid? actualRateCategoryId = null;
            if (!string.IsNullOrWhiteSpace(seedAssignment.ActualRateCategoryCode) &&
                rateCategoryByCode.TryGetValue(seedAssignment.ActualRateCategoryCode, out var foundActualRateCategoryId))
            {
                actualRateCategoryId = foundActualRateCategoryId;
            }

            var assignment = new ResourceAssignment
            {
                Id = Guid.NewGuid(),
                WbsNodeId = node.Id,
                PersonId = personId,
                AssignmentRole = string.IsNullOrWhiteSpace(seedAssignment.AssignmentRole)
                    ? "Contributor"
                    : seedAssignment.AssignmentRole,
                PlannedRateCategoryId = plannedRateCategoryId,
                ActualRateCategoryId = actualRateCategoryId,
                PlannedHours = seedAssignment.PlannedHours,
                ActualHours = seedAssignment.ActualHours,
                Comment = seedAssignment.Comment ?? string.Empty,
                IsActive = seedAssignment.IsActive,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            _dbContext.ResourceAssignments.Add(assignment);
            assignmentsCreated++;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AmprionPqSeedResultDto
        {
            ProjectId = project.Id,
            ProjectNumber = project.ProjectNumber,
            ProjectName = project.Name,
            PersonsUpserted = seedData.Persons.Count,
            RateCategoriesUpserted = seedData.RateCategories.Count,
            TaskStatusesUpserted = seedData.TaskStatuses.Count,
            WbsNodesCreated = orderedNodes.Count,
            ResourceAssignmentsCreated = assignmentsCreated,
            Message = "Amprion-PQ-Seed wurde erfolgreich ausgeführt."
        };
    }

    private async Task<Project> UpsertProject(
        SeedProjectDto seedProject,
        CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects
            .FirstOrDefaultAsync(
                p => p.ProjectNumber == seedProject.ProjectNumber,
                cancellationToken);

        if (project is null)
        {
            project = new Project
            {
                Id = Guid.NewGuid(),
                ProjectNumber = seedProject.ProjectNumber,
                Name = seedProject.Name,
                Description = seedProject.Description,
                PlannedStart = ParseDate(seedProject.PlannedStart),
                PlannedEnd = ParseDate(seedProject.PlannedEnd),
                IsActive = true
            };

            _dbContext.Projects.Add(project);
        }
        else
        {
            project.Name = seedProject.Name;
            project.Description = seedProject.Description;
            project.PlannedStart = ParseDate(seedProject.PlannedStart);
            project.PlannedEnd = ParseDate(seedProject.PlannedEnd);
            project.IsActive = true;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return project;
    }

    private async Task RemoveExistingProjectWbs(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var existingNodeIds = await _dbContext.WbsNodes
            .Where(w => w.ProjectId == projectId)
            .Select(w => w.Id)
            .ToListAsync(cancellationToken);

        if (existingNodeIds.Count == 0)
        {
            return;
        }

        var existingAssignments = await _dbContext.ResourceAssignments
            .Where(a => existingNodeIds.Contains(a.WbsNodeId))
            .ToListAsync(cancellationToken);

        _dbContext.ResourceAssignments.RemoveRange(existingAssignments);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var existingNodes = await _dbContext.WbsNodes
            .Where(w => w.ProjectId == projectId)
            .ToListAsync(cancellationToken);

        _dbContext.WbsNodes.RemoveRange(existingNodes);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task UpsertTaskStatuses(
        List<SeedTaskStatusDto> statuses,
        CancellationToken cancellationToken)
    {
        foreach (var seedStatus in statuses)
        {
            var existing = await _dbContext.TaskStatuses
                .FirstOrDefaultAsync(
                    s => s.Code == seedStatus.Code,
                    cancellationToken);

            if (existing is null)
            {
                _dbContext.TaskStatuses.Add(new WbsTaskStatus
                {
                    Id = Guid.NewGuid(),
                    Code = seedStatus.Code,
                    Label = seedStatus.Label,
                    Color = seedStatus.Color,
                    SortOrder = seedStatus.SortOrder,
                    IsActive = seedStatus.IsActive,
                    IsTerminal = seedStatus.IsTerminal
                });
            }
            else
            {
                existing.Label = seedStatus.Label;
                existing.Color = seedStatus.Color;
                existing.SortOrder = seedStatus.SortOrder;
                existing.IsActive = seedStatus.IsActive;
                existing.IsTerminal = seedStatus.IsTerminal;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task UpsertRateCategories(
        List<SeedRateCategoryDto> rateCategories,
        DateTime now,
        CancellationToken cancellationToken)
    {
        foreach (var seedRateCategory in rateCategories)
        {
            var existing = await _dbContext.RateCategories
                .FirstOrDefaultAsync(
                    r => r.Code == seedRateCategory.Code,
                    cancellationToken);

            if (existing is null)
            {
                _dbContext.RateCategories.Add(new RateCategory
                {
                    Id = Guid.NewGuid(),
                    Code = seedRateCategory.Code,
                    Name = seedRateCategory.Name,
                    HourlyRate = seedRateCategory.HourlyRate,
                    Currency = seedRateCategory.Currency,
                    IsActive = seedRateCategory.IsActive,
                    CreatedAtUtc = now,
                    UpdatedAtUtc = now
                });
            }
            else
            {
                existing.Name = seedRateCategory.Name;
                existing.HourlyRate = seedRateCategory.HourlyRate;
                existing.Currency = seedRateCategory.Currency;
                existing.IsActive = seedRateCategory.IsActive;
                existing.UpdatedAtUtc = now;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task UpsertPersons(
        List<SeedPersonDto> persons,
        DateTime now,
        CancellationToken cancellationToken)
    {
        foreach (var seedPerson in persons)
        {
            var existing = await _dbContext.Persons
                .FirstOrDefaultAsync(
                    p => p.DisplayName == seedPerson.DisplayName,
                    cancellationToken);

            if (existing is null)
            {
                _dbContext.Persons.Add(new Person
                {
                    Id = Guid.NewGuid(),
                    DisplayName = seedPerson.DisplayName,
                    ShortName = seedPerson.ShortName,
                    Email = seedPerson.Email,
                    IsPlaceholder = seedPerson.IsPlaceholder,
                    PlaceholderType = seedPerson.PlaceholderType,
                    IsActive = seedPerson.IsActive,
                    CreatedAtUtc = now,
                    UpdatedAtUtc = now
                });
            }
            else
            {
                existing.ShortName = seedPerson.ShortName;
                existing.Email = seedPerson.Email;
                existing.IsPlaceholder = seedPerson.IsPlaceholder;
                existing.PlaceholderType = seedPerson.PlaceholderType;
                existing.IsActive = seedPerson.IsActive;
                existing.UpdatedAtUtc = now;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Guid? ResolveParentId(
        SeedWbsNodeDto seedNode,
        Dictionary<string, WbsNode> nodeByVisibleWbsId)
    {
        if (string.IsNullOrWhiteSpace(seedNode.ParentVisibleWbsId))
        {
            return null;
        }

        if (nodeByVisibleWbsId.TryGetValue(seedNode.ParentVisibleWbsId, out var parent))
        {
            return parent.Id;
        }

        return null;
    }

    private static WbsNodeType ParseWbsNodeType(string value)
    {
        return value switch
        {
            "MainPackage" => WbsNodeType.MainPackage,
            "SubPackage" => WbsNodeType.SubPackage,
            "Task" => WbsNodeType.Task,
            _ => WbsNodeType.Task
        };
    }

    private static int CalculateLevel(string visibleWbsId)
    {
        if (string.IsNullOrWhiteSpace(visibleWbsId))
        {
            return 1;
        }

        return visibleWbsId.Split('.', StringSplitOptions.RemoveEmptyEntries).Length;
    }

    private static int GetSortOrderFromVisibleWbsId(string visibleWbsId)
    {
        if (string.IsNullOrWhiteSpace(visibleWbsId))
        {
            return 1;
        }

        var lastSegment = visibleWbsId
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault();

        return int.TryParse(lastSegment, out var sortOrder)
            ? sortOrder
            : 1;
    }

    private static DateOnly? ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return DateOnly.TryParse(value, out var parsed)
            ? parsed
            : null;
    }
}

public class AmprionPqSeedData
{
    public string SourceFile { get; set; } = string.Empty;
    public SeedProjectDto Project { get; set; } = new();
    public List<SeedRateCategoryDto> RateCategories { get; set; } = new();
    public List<SeedTaskStatusDto> TaskStatuses { get; set; } = new();
    public List<SeedPersonDto> Persons { get; set; } = new();
    public List<SeedWbsNodeDto> WbsNodes { get; set; } = new();
    public List<SeedResourceAssignmentDto> ResourceAssignments { get; set; } = new();
}

public class SeedProjectDto
{
    public string ProjectNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? PlannedStart { get; set; }
    public string? PlannedEnd { get; set; }
}

public class SeedRateCategoryDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string Currency { get; set; } = "EUR";
    public bool IsActive { get; set; } = true;
}

public class SeedTaskStatusDto
{
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Color { get; set; } = "#94A3B8";
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsTerminal { get; set; }
}

public class SeedPersonDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? Email { get; set; }
    public string? DefaultRole { get; set; }
    public string? DefaultRateCategoryCode { get; set; }
    public bool IsPlaceholder { get; set; }
    public string? PlaceholderType { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SeedWbsNodeDto
{
    public string VisibleWbsId { get; set; } = string.Empty;
    public string? ParentVisibleWbsId { get; set; }
    public int? Level { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public string StatusCode { get; set; } = "Empty";
    public string? PlannedStart { get; set; }
    public string? PlannedEnd { get; set; }
    public int? PlannedDays { get; set; }
    public decimal? PlannedHoursLegacy { get; set; }
    public string? ActualStart { get; set; }
    public string? ActualEnd { get; set; }
    public int? ActualDays { get; set; }
    public decimal? ActualHoursLegacy { get; set; }
    public string? Comment { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SeedResourceAssignmentDto
{
    public string VisibleWbsId { get; set; } = string.Empty;
    public string PersonDisplayName { get; set; } = string.Empty;
    public string AssignmentRole { get; set; } = "Contributor";
    public string? PlannedRateCategoryCode { get; set; }
    public string? ActualRateCategoryCode { get; set; }
    public decimal? PlannedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public string? Comment { get; set; }
    public bool IsActive { get; set; } = true;
}