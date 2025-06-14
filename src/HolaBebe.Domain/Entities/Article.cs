using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class Article : AuditableEntity
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime PublishedAt { get; set; }
    public Guid AuthorId { get; set; }
}
