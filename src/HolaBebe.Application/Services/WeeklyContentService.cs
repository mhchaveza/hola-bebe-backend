using HolaBebe.Application.Dtos;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain;
using HolaBebe.Domain.Entities;
using Mapster;

namespace HolaBebe.Application.Services;

public sealed class WeeklyContentService : IWeeklyContentService
{
    private readonly IUnitOfWork _uow;

    public WeeklyContentService(IUnitOfWork uow) => _uow = uow;

    public async Task<WeeklyContentDto?> GetContentAsync(int week, WeeklyCategory category, CancellationToken ct)
    {
        await foreach (var item in _uow.WeeklyContents.GetAsync(c => c.Week == week && c.Category == category, ct))
        {
            return item.Adapt<WeeklyContentDto>();
        }
        return null;
    }
}
