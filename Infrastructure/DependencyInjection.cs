using Infrastructure.Authentication;
using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Authentication;

namespace Tickest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
             configuration.RegisterServicesFromAssembly(assembly));

        services.AddHttpContextAccessor();

        services.AddScoped<IPermissionProvider, PermissionProvider>();

        RegisterAuthenticationServices(services, configuration);
        RegisterAuthorizationServices(services);

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

                options.RequireHttpsMetadata = false;// no prod, true!
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"])),
                    ClockSkew = TimeSpan.Zero, // Evita a tolerância de tempo 
                    RoleClaimType = ClaimTypes.Role,
                };
            });
    }


    private static void RegisterAuthorizationServices(IServiceCollection services)
    {
        // Registra serviços necessários para o controle de permissões
        services.AddScoped<IPermissionProvider, PermissionProvider>();
        //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        // Configuração opcional de políticas de autorização
        // Exemplo: options.AddPolicy("CreateTicket", policy => policy.Requirements.Add(new PermissionRequirement("CreateTicket")));
        // Exemplo: options.AddPolicy("ManageTickets", policy => policy.Requirements.Add(new PermissionRequirement("ManageTickets")));

    }

    private static void RegisterAuthServices(IServiceCollection services)
    {
        // Registra os serviços necessários para autenticação e fornecimento de tokens
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        //services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}
