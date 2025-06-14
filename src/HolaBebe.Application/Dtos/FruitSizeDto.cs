namespace HolaBebe.Application.Dtos;

public sealed class FruitSizeDto
{
    public int Week { get; init; }
    public string FruitName { get; init; } = string.Empty;
    public double LengthMm { get; init; }
    public double WeightG { get; init; }
    public string? ImageUrl { get; init; }
}
