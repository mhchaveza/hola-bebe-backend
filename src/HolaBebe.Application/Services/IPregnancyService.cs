namespace HolaBebe.Application.Services;

using HolaBebe.Application.Dtos;

public interface IPregnancyService
{
    Task<PregnancyDto> CreatePregnancyAsync(PregnancyDto dto, Guid userId, CancellationToken ct);
    Task<PregnancyDto?> GetCurrentPregnancyAsync(Guid userId, CancellationToken ct);
}
