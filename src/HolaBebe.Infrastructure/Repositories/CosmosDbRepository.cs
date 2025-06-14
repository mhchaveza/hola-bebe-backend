using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace HolaBebe.Infrastructure.Repositories;

public class CosmosDbRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : AuditableEntity
{
    private readonly Container _container;

    public CosmosDbRepository(Container container) => _container = container;

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var response = await _container.ReadItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: ct);
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, [EnumeratorCancellation] CancellationToken ct)
    {
        var query = _container.GetItemLinqQueryable<TEntity>()
            .Where(predicate)
            .ToFeedIterator();
        while (query.HasMoreResults)
        {
            foreach (var item in await query.ReadNextAsync(ct))
            {
                yield return item;
            }
        }
    }

    public async Task AddAsync(TEntity entity, CancellationToken ct)
        => await _container.CreateItemAsync(entity, new PartitionKey(entity.Id.ToString()), cancellationToken: ct);

    public async Task UpdateAsync(TEntity entity, CancellationToken ct)
        => await _container.UpsertItemAsync(entity, new PartitionKey(entity.Id.ToString()), cancellationToken: ct);

    public async Task DeleteAsync(TEntity entity, CancellationToken ct)
        => await _container.DeleteItemAsync<TEntity>(entity.Id.ToString(), new PartitionKey(entity.Id.ToString()), cancellationToken: ct);
}
