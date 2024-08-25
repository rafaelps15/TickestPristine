using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Infrastructure.Services.Auth;

namespace Tickest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
