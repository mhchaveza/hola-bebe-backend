namespace HolaBebe.Application.Dtos;

public sealed class ArticleListDto
{
    public string Slug { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Excerpt { get; init; } = string.Empty;
    public string? CoverImageUrl { get; init; }
    public DateTime PublishedAt { get; init; }
}
