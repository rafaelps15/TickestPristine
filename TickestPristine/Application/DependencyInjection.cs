using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Application.Abstractions.Behaviors;
using Tickest.Application.Abstractions.Services;
using Tickest.Application.Services;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
             configuration.RegisterServicesFromAssembly(assembly));

        // Registra todos os validadores do FluentValidation encontrados no assembly
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<IQueryFilterService, QueryFilterService>();

        return services;
    }
}
