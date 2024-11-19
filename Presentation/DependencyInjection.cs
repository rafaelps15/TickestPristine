using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.Persistence.Repositories;

namespace Tickest.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TickestContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registra repositórios
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IDepartmentRepository, SectorRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        return services;
    }
}
