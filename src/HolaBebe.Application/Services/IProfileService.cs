namespace HolaBebe.Application.Services;

using HolaBebe.Application.Dtos;

public interface IProfileService
{
    Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken ct);
    Task<UserProfileDto> UpsertProfileAsync(UserProfileDto dto, Guid userId, CancellationToken ct);
}
