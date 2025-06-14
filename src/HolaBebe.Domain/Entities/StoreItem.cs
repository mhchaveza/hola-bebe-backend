using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class StoreItem : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PriceCurrency { get; set; } = string.Empty;
    public decimal PriceAmount { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
