using HolaBebe.Application.Dtos;
using HolaBebe.Domain;

namespace HolaBebe.Application.Services;

public interface ICalendarService
{
    IAsyncEnumerable<CalendarEventDto> GetEventsAsync(
        Guid userId,
        DateTime? startFrom,
        DateTime? startTo,
        CalendarType? type,
        CancellationToken ct);

    Task<CalendarEventDto> CreateEventAsync(CalendarEventCreateDto dto, Guid userId, CancellationToken ct);
    Task<CalendarEventDto?> GetEventAsync(Guid id, Guid userId, CancellationToken ct);
    Task<CalendarEventDto?> UpdateEventAsync(Guid id, CalendarEventUpdateDto dto, Guid userId, CancellationToken ct);
    Task<bool> DeleteEventAsync(Guid id, Guid userId, CancellationToken ct);
}
