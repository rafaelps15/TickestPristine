using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Domain.Contracts.Services;
using Tickest.Infrastructure.Helpers;
using Tickest.Infrastructure.Services.Auth;
using Tickest.Infrastructure.Services.Authentication;

namespace Tickest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Adiciona suporte ao HttpContext
        services.AddHttpContextAccessor();

        // Registra serviços de autenticação
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();

        // Registra serviços auxiliares
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
