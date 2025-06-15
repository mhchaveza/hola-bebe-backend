using HolaBebe.Domain.Entities;

namespace HolaBebe.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<UserProfile> UserProfiles { get; }
    IGenericRepository<Pregnancy> Pregnancies { get; }
    IGenericRepository<FruitSizeCatalog> FruitSizes { get; }
    IGenericRepository<WeeklyContent> WeeklyContents { get; }
    IGenericRepository<CalendarEvent> CalendarEvents { get; }
    IGenericRepository<Article> Articles { get; }
    IGenericRepository<TutorialSlide> TutorialSlides { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}
