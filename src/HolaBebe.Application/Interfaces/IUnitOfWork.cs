using HolaBebe.Domain.Entities;

namespace HolaBebe.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<UserProfile> UserProfiles { get; }
    IGenericRepository<Pregnancy> Pregnancies { get; }
    IGenericRepository<FruitSizeCatalog> FruitSizes { get; }
    IGenericRepository<WeeklyContent> WeeklyContents { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}
