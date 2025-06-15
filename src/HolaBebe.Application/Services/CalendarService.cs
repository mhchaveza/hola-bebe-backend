using HolaBebe.Application.Dtos;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain.Entities;
using HolaBebe.Domain;
using Mapster;
using System.Runtime.CompilerServices;

namespace HolaBebe.Application.Services;

public sealed class CalendarService : ICalendarService
{
    private readonly IUnitOfWork _uow;

    public CalendarService(IUnitOfWork uow) => _uow = uow;

    public async Task<CalendarEventDto> CreateEventAsync(CalendarEventCreateDto dto, Guid userId, CancellationToken ct)
    {
        var entity = dto.Adapt<CalendarEvent>();
        entity.UserId = userId;
        await _uow.CalendarEvents.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity.Adapt<CalendarEventDto>();
    }

    public async Task<bool> DeleteEventAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var entity = await _uow.CalendarEvents.GetByIdAsync(id, ct);
        if (entity is null || entity.UserId != userId)
        {
            return false;
        }

        await _uow.CalendarEvents.DeleteAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }

    public async IAsyncEnumerable<CalendarEventDto> GetEventsAsync(
        Guid userId,
        DateTime? startFrom,
        DateTime? startTo,
        CalendarType? type,
        [EnumeratorCancellation] CancellationToken ct)
    {
        await foreach (var entity in _uow.CalendarEvents.GetAsync(e =>
            e.UserId == userId &&
            (!startFrom.HasValue || e.StartDateTime >= startFrom.Value) &&
            (!startTo.HasValue || e.StartDateTime <= startTo.Value) &&
            (!type.HasValue || e.Type == type.Value), ct))
        {
            yield return entity.Adapt<CalendarEventDto>();
        }
    }

    public async Task<CalendarEventDto?> GetEventAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var entity = await _uow.CalendarEvents.GetByIdAsync(id, ct);
        return entity is null || entity.UserId != userId
            ? null
            : entity.Adapt<CalendarEventDto>();
    }

    public async Task<CalendarEventDto?> UpdateEventAsync(Guid id, CalendarEventUpdateDto dto, Guid userId, CancellationToken ct)
    {
        var entity = await _uow.CalendarEvents.GetByIdAsync(id, ct);
        if (entity is null || entity.UserId != userId)
        {
            return null;
        }

        if (dto.Title is not null)
        {
            entity.Title = dto.Title;
        }

        if (dto.Description is not null)
        {
            entity.Description = dto.Description;
        }

        if (dto.StartDateTime.HasValue)
        {
            entity.StartDateTime = dto.StartDateTime.Value;
        }

        if (dto.EndDateTime.HasValue)
        {
            entity.EndDateTime = dto.EndDateTime.Value;
        }

        if (dto.Type.HasValue)
        {
            entity.Type = dto.Type.Value;
        }

        if (dto.Color is not null)
        {
            entity.Color = dto.Color;
        }

        if (dto.PregnancyId.HasValue)
        {
            entity.PregnancyId = dto.PregnancyId.Value;
        }

        if (dto.Location is not null)
        {
            entity.Location = dto.Location;
        }

        entity.Touch();
        await _uow.CalendarEvents.UpdateAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity.Adapt<CalendarEventDto>();
    }
}
