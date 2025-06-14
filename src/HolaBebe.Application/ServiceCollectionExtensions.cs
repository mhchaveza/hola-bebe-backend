using HolaBebe.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HolaBebe.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IPregnancyService, PregnancyService>();
        return services;
    }
}
