using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Infrastructure.Authentication;
using System.Text;
using Tickest.Domain.Exceptions;

namespace Tickest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Adiciona suporte ao HttpContext para acessos rápidos ao contexto da aplicação
        services.AddHttpContextAccessor();

        // Registra serviços de autenticação, autorização e segurança
        RegisterAuthenticationServices(services, configuration);
        RegisterAuthorizationServices(services);

        // Registra os serviços de gerenciamento de tokens e autenticação
        RegisterAuthServices(services);

        return services;
    }

    private static void RegisterAuthenticationServices(IServiceCollection services, IConfiguration configuration)
    {
        // Configura o JwtBearer para autenticação baseada em token JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

                if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
                {
                    throw new TickestException("O segredo JWT está ausente ou é nulo na configuração.");
                }

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero // Evita a tolerância de tempo 
                };
            });
    }


    private static void RegisterAuthorizationServices(IServiceCollection services)
    {
        // Registra serviços necessários para o controle de permissões
        services.AddScoped<IPermissionProvider, PermissionProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        // Configuração opcional de políticas de autorização
        // Exemplo: options.AddPolicy("CreateTicket", policy => policy.Requirements.Add(new PermissionRequirement("CreateTicket")));
        // Exemplo: options.AddPolicy("ManageTickets", policy => policy.Requirements.Add(new PermissionRequirement("ManageTickets")));
    }

    private static void RegisterAuthServices(IServiceCollection services)
    {
        // Registra os serviços necessários para autenticação e fornecimento de tokens
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}
