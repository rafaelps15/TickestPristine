using Microsoft.Extensions.DependencyInjection;

namespace Tickest.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Register repositories
        //services.AddScoped<IAreaRepository, AreaRepository>();



        // Register any other domain services or interfaces here

        return services;
    }
}
