using HolaBebe.Domain;

namespace HolaBebe.Application.Dtos;

public sealed class UserProfileDto
{
    public Guid Id { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string? PhotoUrl { get; init; }
    public DateTime BirthDate { get; init; }
    public Gender Gender { get; init; }
    public string? Country { get; init; }
    public string? Phone { get; init; }
    public string? Goals { get; init; }
    public string? Interests { get; init; }
}
