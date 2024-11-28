using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Infrastructure.Authentication;

namespace Tickest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Adiciona suporte ao HttpContext
        services.AddHttpContextAccessor();

        // Registra serviços de autenticação e autorização
        services.AddScoped<IPermissionProvider, PermissionProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


        // Adicionar serviços de autenticação e outros
        //services.AddScoped<IAuthService, Authenticator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Configuração de políticas de autorização
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CreateTicket", policy => policy.Requirements.Add(new PermissionRequirement("CreateTicket")));
            options.AddPolicy("ManageTickets", policy => policy.Requirements.Add(new PermissionRequirement("ManageTickets")));
        });

        return services;
    }
}
