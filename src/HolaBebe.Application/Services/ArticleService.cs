using HolaBebe.Application.Dtos;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain.Entities;
using Mapster;

namespace HolaBebe.Application.Services;

public sealed class ArticleService : IArticleService
{
    private readonly IUnitOfWork _uow;

    public ArticleService(IUnitOfWork uow) => _uow = uow;

    public async Task<PagedResultDto<ArticleListDto>> GetArticlesAsync(int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var list = new List<ArticleListDto>();
        var all = new List<Article>();
        await foreach (var a in _uow.Articles.GetAsync(_ => true, ct))
        {
            all.Add(a);
        }

        var total = all.Count;
        foreach (var item in all.OrderByDescending(a => a.PublishedAt).Skip((page - 1) * pageSize).Take(pageSize))
        {
            list.Add(item.Adapt<ArticleListDto>());
        }

        return new PagedResultDto<ArticleListDto>
        {
            Items = list,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ArticleDto?> GetArticleAsync(string slug, CancellationToken ct)
    {
        await foreach (var a in _uow.Articles.GetAsync(a => a.Slug == slug, ct))
        {
            return a.Adapt<ArticleDto>();
        }
        return null;
    }
}
