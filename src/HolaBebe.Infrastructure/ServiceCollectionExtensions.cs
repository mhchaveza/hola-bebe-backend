using HolaBebe.Application.Interfaces;
using HolaBebe.Infrastructure.Data;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HolaBebe.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CosmosSettings>(config.GetSection("Cosmos"));
        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CosmosSettings>>().Value;
            return new CosmosClient(settings.ConnectionString);
        });
        services.AddSingleton<IUnitOfWork, CosmosDbUnitOfWork>();
        return services;
    }
}
