namespace HolaBebe.Application.Dtos;

public sealed class TutorialSlideDto
{
    public int Order { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Subtitle { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}
