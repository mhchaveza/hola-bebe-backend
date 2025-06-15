using HolaBebe.Application.Dtos;

namespace HolaBebe.Application.Services;

public interface IArticleService
{
    Task<PagedResultDto<ArticleListDto>> GetArticlesAsync(int page, int pageSize, CancellationToken ct);
    Task<ArticleDto?> GetArticleAsync(string slug, CancellationToken ct);
}
