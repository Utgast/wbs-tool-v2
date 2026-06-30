namespace WbsTool.Api.Modules.Wbs.Contracts;

public class WbsTreeNodeDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? ParentId { get; set; }

    public string VisibleWbsId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    public int Level { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    public Guid? StatusId { get; set; }
    public string? StatusLabel { get; set; }

    public DateOnly? PlannedStart { get; set; }
    public DateOnly? PlannedEnd { get; set; }

    public decimal PlannedHoursTotal { get; set; }
    public decimal ActualHoursTotal { get; set; }
    public decimal PlannedCostTotal { get; set; }
    public decimal ActualCostTotal { get; set; }

    public bool IsBlocked { get; set; }
    public string Comment { get; set; } = string.Empty;

    public List<WbsTreeNodeDto> Children { get; set; } = new();
}