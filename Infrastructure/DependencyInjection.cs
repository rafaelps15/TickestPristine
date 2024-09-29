using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Infrastructure.Interfaces;
using Tickest.Infrastructure.Services.Auth;

namespace Tickest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
