using HolaBebe.Application.Dtos;
using HolaBebe.Application.Interfaces;
using Mapster;

namespace HolaBebe.Application.Services;

public sealed class TutorialService : ITutorialService
{
    private readonly IUnitOfWork _uow;

    public TutorialService(IUnitOfWork uow) => _uow = uow;

    public async IAsyncEnumerable<TutorialSlideDto> GetSlidesAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        var list = new List<TutorialSlideDto>();
        await foreach (var slide in _uow.TutorialSlides.GetAsync(_ => true, ct))
        {
            list.Add(slide.Adapt<TutorialSlideDto>());
        }
        foreach (var item in list.OrderBy(s => s.Order))
        {
            yield return item;
        }
    }
}
