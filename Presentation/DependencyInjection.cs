using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Application.Abstractions.Data;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.Persistence.Repositories;
using Tickest.Persistence.Seeders;

namespace Tickest.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Registra o DbContext para acesso ao banco de dados com a conexão definida no arquivo de configuração
        services.AddDbContext<TickestContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registra os repositórios específicos, incluindo o BaseRepository com a sintaxe genérica
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<ISectorRepository, SectorRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IApplicationDbContext, TickestContext>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Registra o serviço que será utilizado para popular o banco de dados na primeira execução
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        return services;
    }
}
