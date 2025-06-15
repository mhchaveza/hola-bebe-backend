using HolaBebe.Application.Dtos;
using HolaBebe.Domain;

namespace HolaBebe.Application.Services;

public interface IWeeklyContentService
{
    Task<WeeklyContentDto?> GetContentAsync(int week, WeeklyCategory category, CancellationToken ct);
}
