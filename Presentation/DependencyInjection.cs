using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tickest.Domain.Repositories;
using Tickest.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Tickest.Persistence.Repositories;

namespace Tickest.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TickestContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Registra repositórios
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<ISetorRepository, SetorRepository>();

            return services;
        }
    }
}
