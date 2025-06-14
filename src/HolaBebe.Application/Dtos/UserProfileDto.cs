using HolaBebe.Domain;

namespace HolaBebe.Application.Dtos;

public sealed class UserProfileDto
{
    public string DisplayName { get; init; } = string.Empty;
    public string? PhotoUrl { get; init; }
    public DateTime BirthDate { get; init; }
    public Gender Gender { get; init; }
    public string? Country { get; init; }
    public string? Phone { get; init; }
    public ICollection<GoalType> Goals { get; init; } = Array.Empty<GoalType>();
    public ICollection<InterestType> Interests { get; init; } = Array.Empty<InterestType>();
}
