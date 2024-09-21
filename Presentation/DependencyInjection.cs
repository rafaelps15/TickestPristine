using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Persistence.Repositories;

namespace Tickest.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TickestContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IBaseRepotirory<>), typeof(BaseRepository<>));

        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IMensagemRepository, MensagemRepository>();
        services.AddScoped<ISetorRepository, SetorRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}
