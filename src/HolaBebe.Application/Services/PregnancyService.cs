using HolaBebe.Application.Dtos;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain.Entities;
using Mapster;

namespace HolaBebe.Application.Services;

public sealed class PregnancyService : IPregnancyService
{
    private readonly IUnitOfWork _uow;

    public PregnancyService(IUnitOfWork uow) => _uow = uow;

    public async Task<PregnancyDto> CreatePregnancyAsync(PregnancyDto dto, Guid userId, CancellationToken ct)
    {
        var entity = dto.Adapt<Pregnancy>();
        entity.UserId = userId;
        await _uow.Pregnancies.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity.Adapt<PregnancyDto>();
    }

    public async Task<PregnancyDto?> GetCurrentPregnancyAsync(Guid userId, CancellationToken ct)
    {
        await foreach (var entity in _uow.Pregnancies.GetAsync(p => p.UserId == userId && p.Current, ct))
        {
            return entity.Adapt<PregnancyDto>();
        }
        return null;
    }
}
