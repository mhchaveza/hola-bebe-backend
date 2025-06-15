using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class WeeklyContent : AuditableEntity
{
    public int Week { get; set; }
    public WeeklyCategory Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
}
