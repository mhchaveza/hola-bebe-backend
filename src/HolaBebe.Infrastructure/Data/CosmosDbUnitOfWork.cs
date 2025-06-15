using Microsoft.Azure.Cosmos;
using HolaBebe.Application.Interfaces;
using HolaBebe.Domain.Entities;
using HolaBebe.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace HolaBebe.Infrastructure.Data;

public sealed class CosmosDbUnitOfWork : IUnitOfWork, IDisposable
{
    private readonly CosmosClient _client;
    private readonly Database _database;

    public IGenericRepository<UserProfile> UserProfiles { get; }
    public IGenericRepository<Pregnancy> Pregnancies { get; }
    public IGenericRepository<FruitSizeCatalog> FruitSizes { get; }
    public IGenericRepository<WeeklyContent> WeeklyContents { get; }
    public IGenericRepository<CalendarEvent> CalendarEvents { get; }
    public IGenericRepository<Article> Articles { get; }
    public IGenericRepository<TutorialSlide> TutorialSlides { get; }

    public CosmosDbUnitOfWork(CosmosClient client, IOptions<CosmosSettings> options)
    {
        _client = client;
        _database = _client.CreateDatabaseIfNotExistsAsync(options.Value.DatabaseId).GetAwaiter().GetResult();
        UserProfiles = new CosmosDbRepository<UserProfile>(_database.CreateContainerIfNotExistsAsync("UserProfiles", "/id").GetAwaiter().GetResult());
        Pregnancies = new CosmosDbRepository<Pregnancy>(_database.CreateContainerIfNotExistsAsync("Pregnancies", "/id").GetAwaiter().GetResult());
        FruitSizes = new CosmosDbRepository<FruitSizeCatalog>(_database.CreateContainerIfNotExistsAsync("FruitSizes", "/id").GetAwaiter().GetResult());
        WeeklyContents = new CosmosDbRepository<WeeklyContent>(_database.CreateContainerIfNotExistsAsync("WeeklyContents", "/id").GetAwaiter().GetResult());
        CalendarEvents = new CosmosDbRepository<CalendarEvent>(_database.CreateContainerIfNotExistsAsync("CalendarEvents", "/id").GetAwaiter().GetResult());
        Articles = new CosmosDbRepository<Article>(_database.CreateContainerIfNotExistsAsync("Articles", "/id").GetAwaiter().GetResult());
        TutorialSlides = new CosmosDbRepository<TutorialSlide>(_database.CreateContainerIfNotExistsAsync("TutorialSlides", "/id").GetAwaiter().GetResult());
    }

    public Task<int> SaveChangesAsync(CancellationToken ct) => Task.FromResult(0);

    public void Dispose() => _client.Dispose();
}
