namespace HolaBebe.Application.Dtos;

public sealed class WeeklyContentDto
{
    public string Title { get; init; } = string.Empty;
    public string HtmlContent { get; init; } = string.Empty;
    public string? VideoUrl { get; init; }
}
