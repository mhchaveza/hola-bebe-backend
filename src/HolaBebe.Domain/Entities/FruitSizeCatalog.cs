using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class FruitSizeCatalog : AuditableEntity
{
    public int Week { get; set; }
    public string FruitName { get; set; } = string.Empty;
    public double LengthMm { get; set; }
    public double WeightG { get; set; }
    public string? ImageUrl { get; set; }
}
