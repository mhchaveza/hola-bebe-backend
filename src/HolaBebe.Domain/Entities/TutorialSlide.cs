using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class TutorialSlide : AuditableEntity
{
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
