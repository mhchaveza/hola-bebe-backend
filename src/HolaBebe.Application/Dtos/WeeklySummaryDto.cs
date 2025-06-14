namespace HolaBebe.Application.Dtos;

public sealed class WeeklySummaryDto
{
    public int Week { get; init; }
    public string Baby { get; init; } = string.Empty;
    public string Mom { get; init; } = string.Empty;
    public string Nutrition { get; init; } = string.Empty;
    public string Tips { get; init; } = string.Empty;
    public FruitSizeDto? FruitSize { get; init; }
}
