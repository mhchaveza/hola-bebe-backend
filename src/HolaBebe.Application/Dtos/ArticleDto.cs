namespace HolaBebe.Application.Dtos;

public sealed class ArticleDto
{
    public string Title { get; init; } = string.Empty;
    public string HtmlContent { get; init; } = string.Empty;
    public ICollection<string> Tags { get; init; } = Array.Empty<string>();
    public string Author { get; init; } = string.Empty;
    public DateTime PublishedAt { get; init; }
}
