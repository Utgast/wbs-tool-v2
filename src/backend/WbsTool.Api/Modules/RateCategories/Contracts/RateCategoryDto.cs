namespace WbsTool.Api.Modules.RateCategories.Contracts;

public class RateCategoryDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public decimal HourlyRate { get; set; }
    public string Currency { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}