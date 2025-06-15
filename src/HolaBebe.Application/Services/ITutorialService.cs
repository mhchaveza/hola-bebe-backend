using HolaBebe.Application.Dtos;

namespace HolaBebe.Application.Services;

public interface ITutorialService
{
    IAsyncEnumerable<TutorialSlideDto> GetSlidesAsync(CancellationToken ct);
}
