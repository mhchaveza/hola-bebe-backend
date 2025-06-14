using HolaBebe.Domain.Entities;

namespace HolaBebe.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<UserProfile> UserProfiles { get; }
    IGenericRepository<Pregnancy> Pregnancies { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}
