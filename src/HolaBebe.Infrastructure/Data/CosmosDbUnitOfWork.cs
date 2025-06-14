using Azure.Cosmos;
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

    public CosmosDbUnitOfWork(CosmosClient client, IOptions<CosmosSettings> options)
    {
        _client = client;
        _database = _client.CreateDatabaseIfNotExistsAsync(options.Value.DatabaseId).GetAwaiter().GetResult();
        UserProfiles = new CosmosDbRepository<UserProfile>(_database.CreateContainerIfNotExistsAsync("UserProfiles", "/id").GetAwaiter().GetResult());
        Pregnancies = new CosmosDbRepository<Pregnancy>(_database.CreateContainerIfNotExistsAsync("Pregnancies", "/id").GetAwaiter().GetResult());
    }

    public Task<int> SaveChangesAsync(CancellationToken ct) => Task.FromResult(0);

    public void Dispose() => _client.Dispose();
}
