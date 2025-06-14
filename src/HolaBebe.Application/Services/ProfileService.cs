using HolaBebe.Application.Dtos;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain.Entities;
using Mapster;

namespace HolaBebe.Application.Services;

public sealed class ProfileService : IProfileService
{
    private readonly IUnitOfWork _uow;

    public ProfileService(IUnitOfWork uow) => _uow = uow;

    public async Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken ct)
    {
        await foreach (var entity in _uow.UserProfiles.GetAsync(p => p.UserId == userId, ct))
        {
            return entity.Adapt<UserProfileDto>();
        }
        return null;
    }

    public async Task<UserProfileDto> UpsertProfileAsync(UserProfileDto dto, Guid userId, CancellationToken ct)
    {
        UserProfile? entity = null;
        await foreach (var p in _uow.UserProfiles.GetAsync(p => p.UserId == userId, ct))
        {
            entity = p;
            break;
        }

        if (entity is null)
        {
            entity = dto.Adapt<UserProfile>();
            entity.UserId = userId;
            await _uow.UserProfiles.AddAsync(entity, ct);
        }
        else
        {
            entity.DisplayName = dto.DisplayName;
            entity.PhotoUrl = dto.PhotoUrl;
            entity.BirthDate = dto.BirthDate;
            entity.Gender = dto.Gender;
            entity.Country = dto.Country;
            entity.Phone = dto.Phone;
            entity.Goals = dto.Goals;
            entity.Interests = dto.Interests;
            entity.Touch();
            await _uow.UserProfiles.UpdateAsync(entity, ct);
        }

        await _uow.SaveChangesAsync(ct);
        return entity.Adapt<UserProfileDto>();
    }
}
