namespace HolaBebe.Application.Services;

using HolaBebe.Application.Dtos;

public interface IPregnancyService
{
    Task<PregnancyDto> CreatePregnancyAsync(PregnancyCreateDto dto, Guid userId, CancellationToken ct);
    Task<PregnancyDto?> GetCurrentPregnancyAsync(Guid userId, CancellationToken ct);
    Task<PregnancyDto?> UpdatePregnancyAsync(Guid id, PregnancyUpdateDto dto, Guid userId, CancellationToken ct);
    Task<FruitSizeDto?> GetFruitSizeAsync(Guid id, Guid userId, CancellationToken ct);
    Task<WeeklySummaryDto?> GetWeeklySummaryAsync(Guid id, int week, Guid userId, CancellationToken ct);
}
