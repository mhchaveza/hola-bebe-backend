using HolaBebe.Domain;

namespace HolaBebe.Application.Dtos;

public sealed class CalendarEventUpdateDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDateTime { get; init; }
    public DateTime? EndDateTime { get; init; }
    public CalendarType? Type { get; init; }
    public string? Color { get; init; }
    public Guid? PregnancyId { get; init; }
    public string? Location { get; init; }
}
