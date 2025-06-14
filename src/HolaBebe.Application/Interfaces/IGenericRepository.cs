using System.Linq.Expressions;
using HolaBebe.Domain.Entities;

namespace HolaBebe.Application.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : AuditableEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task AddAsync(TEntity entity, CancellationToken ct);
    Task UpdateAsync(TEntity entity, CancellationToken ct);
    Task DeleteAsync(TEntity entity, CancellationToken ct);
}
