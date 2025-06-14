using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class CalendarEvent : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public CalendarType Type { get; set; }
    public string? Color { get; set; }
    public Guid? PregnancyId { get; set; }
    public string? Location { get; set; }
}
