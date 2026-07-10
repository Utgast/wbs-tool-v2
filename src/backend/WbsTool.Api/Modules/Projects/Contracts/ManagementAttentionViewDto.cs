namespace WbsTool.Api.Modules.Projects.Contracts;

public class ManagementAttentionViewDto
{
    public string Type { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
    public Guid SourceId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int Impact { get; set; }
    public int Urgency { get; set; }
    public int DependencyEffect { get; set; }
    public int Recoverability { get; set; }
    public int Confidence { get; set; }

    public decimal PriorityScore { get; set; }

    public Guid? SuggestedOwnerPersonId { get; set; }
    public string SuggestedAction { get; set; } = string.Empty;

    public string Explanation { get; set; } = string.Empty;
    public DateOnly? ReactionDate { get; set; }
}
