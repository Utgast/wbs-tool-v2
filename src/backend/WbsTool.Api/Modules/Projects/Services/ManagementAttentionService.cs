using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Deliverables.Models;
using WbsTool.Api.Modules.Projects.Contracts;
using WbsTool.Api.Modules.Risks.Models;

namespace WbsTool.Api.Modules.Projects.Services;

public class ManagementAttentionService : IManagementAttentionService
{
    private readonly AppDbContext _dbContext;

    public ManagementAttentionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<ManagementAttentionViewDto> GetAttentionItems(Guid projectId)
    {
        var projectExists = _dbContext.Projects
            .AsNoTracking()
            .Any(p => p.Id == projectId && p.IsActive);

        if (!projectExists)
        {
            return [];
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        var items = new List<ManagementAttentionViewDto>();

        items.AddRange(BuildRiskAttentionItems(projectId, today));
        items.AddRange(BuildOverdueDeliverableAttentionItems(projectId, today));

        // Sprint-037 scope: only placeholders for future sources.
        items.AddRange(BuildCompetencyGapPlaceholders(projectId));
        items.AddRange(BuildResourceGapPlaceholders(projectId));

        return items
            .OrderByDescending(x => x.PriorityScore)
            .ThenBy(x => x.ReactionDate)
            .ToList();
    }

    private List<ManagementAttentionViewDto> BuildRiskAttentionItems(Guid projectId, DateOnly today)
    {
        var risks = _dbContext.Risks
            .AsNoTracking()
            .Where(r =>
                r.ProjectId == projectId &&
                r.Status != RiskStatus.Accepted &&
                r.Status != RiskStatus.Closed)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        return risks.Select(r =>
        {
            bool isCritical = r.Severity == RiskSeverity.High;

            return new ManagementAttentionViewDto
            {
                Type = "Risk",
                SourceType = "Risk",
                SourceId = r.Id,
                Title = r.Title,
                Description = r.Description ?? string.Empty,

                PriorityScore = isCritical
                    ? ManagementAttentionDefaults.CriticalRiskPriorityScore
                    : ManagementAttentionDefaults.OpenRiskPriorityScore,
                Impact = isCritical ? 5 : 4,
                Urgency = isCritical ? 5 : 3,
                DependencyEffect = 3,
                Recoverability = 3,
                Confidence = 4,

                SuggestedOwnerPersonId = r.OwnerPersonId,

                SuggestedAction = isCritical
                    ? ManagementAttentionDefaults.CriticalRiskSuggestedAction
                    : ManagementAttentionDefaults.OpenRiskSuggestedAction,

                Explanation = isCritical
                    ? $"Risiko '{r.Title}' ist mit Schweregrad High eingestuft und noch offen. Unmittelbarer Handlungsbedarf."
                    : $"Risiko '{r.Title}' ist offen und wurde noch nicht abgeschlossen. Regelmaessige Ueberwachung erforderlich.",

                ReactionDate = today.AddDays(isCritical
                    ? ManagementAttentionDefaults.CriticalRiskReactionDays
                    : ManagementAttentionDefaults.OpenRiskReactionDays)
            };
        }).ToList();
    }

    private List<ManagementAttentionViewDto> BuildOverdueDeliverableAttentionItems(
        Guid projectId,
        DateOnly today)
    {
        var deliverables = _dbContext.Deliverables
            .AsNoTracking()
            .Where(d =>
                d.ProjectId == projectId &&
                d.Status != DeliverableStatus.Delivered &&
                d.DueDate < today)
            .OrderBy(d => d.DueDate)
            .ToList();

        return deliverables.Select(d => new ManagementAttentionViewDto
        {
            Type = "Deliverable",
            SourceType = "Deliverable",
            SourceId = d.Id,
            Title = d.Name,
            Description = d.Description ?? string.Empty,

            PriorityScore = ManagementAttentionDefaults.OverdueDeliverablePriorityScore,
            Impact = 5,
            Urgency = 5,
            DependencyEffect = 4,
            Recoverability = 2,
            Confidence = 4,

            SuggestedOwnerPersonId = d.OwnerPersonId,
            SuggestedAction = ManagementAttentionDefaults.OverdueDeliverableSuggestedAction,

            Explanation = $"Deliverable '{d.Name}' war faellig am {d.DueDate:dd.MM.yyyy} und ist noch nicht abgeliefert. Faelligkeitsdatum ueberschritten.",

            ReactionDate = today.AddDays(ManagementAttentionDefaults.OverdueDeliverableReactionDays)
        }).ToList();
    }

    private static List<ManagementAttentionViewDto> BuildCompetencyGapPlaceholders(Guid projectId)
    {
        // Sprint-037 scope: competency-gap generation is intentionally deferred.
        _ = projectId;
        return [];
    }

    private static List<ManagementAttentionViewDto> BuildResourceGapPlaceholders(Guid projectId)
    {
        // Sprint-037 scope: resource-gap generation is intentionally deferred.
        _ = projectId;
        return [];
    }
}
