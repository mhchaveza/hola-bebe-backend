using HolaBebe.Domain;

namespace HolaBebe.Application.Dtos;

public sealed class CalendarEventDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime StartDateTime { get; init; }
    public CalendarType Type { get; init; }
    public string? Color { get; init; }
}
