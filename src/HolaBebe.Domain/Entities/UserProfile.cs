using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class UserProfile : AuditableEntity
{
    public string DisplayName { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Goals { get; set; }
    public string? Interests { get; set; }
}
