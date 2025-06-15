using HolaBebe.Application.Dtos;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain.Entities;
using HolaBebe.Domain;
using Mapster;

namespace HolaBebe.Application.Services;

public sealed class PregnancyService : IPregnancyService
{
    private readonly IUnitOfWork _uow;

    public PregnancyService(IUnitOfWork uow) => _uow = uow;

    public async Task<PregnancyDto> CreatePregnancyAsync(PregnancyCreateDto dto, Guid userId, CancellationToken ct)
    {
        await foreach (var _ in _uow.Pregnancies.GetAsync(p => p.UserId == userId && p.Current, ct))
        {
            throw new InvalidOperationException("Pregnancy already exists");
        }

        var (days, method) = ComputeGestationalAge(dto);

        var entity = new Pregnancy
        {
            UserId = userId,
            Current = true,
            GestationalAgeDays = days,
            ConceptionDate = dto.ConceptionDate,
            DueDate = dto.DueDate,
            LastMenstruationDate = dto.LastMenstruationDate,
            EstimationMethod = method
        };

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

    public async Task<PregnancyDto?> UpdatePregnancyAsync(Guid id, PregnancyUpdateDto dto, Guid userId, CancellationToken ct)
    {
        var entity = await _uow.Pregnancies.GetByIdAsync(id, ct);
        if (entity is null || entity.UserId != userId)
        {
            return null;
        }

        if (dto.Current.HasValue)
        {
            entity.Current = dto.Current.Value;
        }

        if (dto.GestationalAgeDays.HasValue)
        {
            entity.GestationalAgeDays = dto.GestationalAgeDays.Value;
            entity.EstimationMethod = EstimationMethod.Ga;
        }
        else if (dto.ConceptionDate.HasValue)
        {
            entity.ConceptionDate = dto.ConceptionDate;
            entity.GestationalAgeDays = (int)(DateTime.UtcNow.Date - dto.ConceptionDate.Value.Date).TotalDays;
            entity.EstimationMethod = EstimationMethod.Conception;
        }
        else if (dto.LastMenstruationDate.HasValue)
        {
            entity.LastMenstruationDate = dto.LastMenstruationDate;
            entity.GestationalAgeDays = (int)(DateTime.UtcNow.Date - dto.LastMenstruationDate.Value.Date).TotalDays;
            entity.EstimationMethod = EstimationMethod.Lmp;
        }
        else if (dto.DueDate.HasValue)
        {
            entity.DueDate = dto.DueDate;
            entity.GestationalAgeDays = 280 - (int)(dto.DueDate.Value.Date - DateTime.UtcNow.Date).TotalDays;
            entity.EstimationMethod = EstimationMethod.Due;
        }

        entity.Touch();
        await _uow.Pregnancies.UpdateAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity.Adapt<PregnancyDto>();
    }

    public async Task<FruitSizeDto?> GetFruitSizeAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var preg = await _uow.Pregnancies.GetByIdAsync(id, ct);
        if (preg is null || preg.UserId != userId)
        {
            return null;
        }

        await foreach (var fs in _uow.FruitSizes.GetAsync(f => f.Week == preg.WeekNumber, ct))
        {
            return fs.Adapt<FruitSizeDto>();
        }

        return null;
    }

    public async Task<WeeklySummaryDto?> GetWeeklySummaryAsync(Guid id, int week, Guid userId, CancellationToken ct)
    {
        if (week < 1 || week > 42)
        {
            return null;
        }

        var preg = await _uow.Pregnancies.GetByIdAsync(id, ct);
        if (preg is null || preg.UserId != userId)
        {
            return null;
        }

        var summary = new WeeklySummaryDto { Week = week };

        await foreach (var content in _uow.WeeklyContents.GetAsync(c => c.Week == week, ct))
        {
            switch (content.Category)
            {
                case WeeklyCategory.Baby:
                    summary.Baby = content.HtmlContent;
                    break;
                case WeeklyCategory.Mom:
                    summary.Mom = content.HtmlContent;
                    break;
                case WeeklyCategory.Nutrition:
                    summary.Nutrition = content.HtmlContent;
                    break;
                case WeeklyCategory.Tips:
                    summary.Tips = content.HtmlContent;
                    break;
            }
        }

        await foreach (var fs in _uow.FruitSizes.GetAsync(f => f.Week == week, ct))
        {
            summary.FruitSize = fs.Adapt<FruitSizeDto>();
            break;
        }

        return summary;
    }

    private static (int days, EstimationMethod method) ComputeGestationalAge(PregnancyCreateDto dto)
    {
        var today = DateTime.UtcNow.Date;

        if (dto.GestationalAgeDays.HasValue)
        {
            return (dto.GestationalAgeDays.Value, EstimationMethod.Ga);
        }

        if (dto.ConceptionDate.HasValue)
        {
            return ((int)(today - dto.ConceptionDate.Value.Date).TotalDays, EstimationMethod.Conception);
        }

        if (dto.LastMenstruationDate.HasValue)
        {
            return ((int)(today - dto.LastMenstruationDate.Value.Date).TotalDays, EstimationMethod.Lmp);
        }

        if (dto.DueDate.HasValue)
        {
            return (280 - (int)(dto.DueDate.Value.Date - today).TotalDays, EstimationMethod.Due);
        }

        return (0, EstimationMethod.Unknown);
    }
}
