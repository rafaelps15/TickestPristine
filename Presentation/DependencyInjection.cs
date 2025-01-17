using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Data;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.Persistence.Repositories;
using Tickest.Persistence.Seeders;


namespace Tickest.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Configura o DbContext
        services.AddDbContext<TickestContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registra a interface IApplicationDbContext
        services.AddScoped<IApplicationDbContext, TickestContext>();

        // Registra os repositórios
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<ISectorRepository, SectorRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IApplicationSettingRepository, ApplicationSettingRepository>();

        // Registra a unidade de trabalho
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Registra o seeder
        //services.AddScoped<RoleSeeder>();

        // Registra o serviço para popular o banco de dados na primeira execução
        //services.AddScoped<ILogger<DatabaseSeeder>, Logger<DatabaseSeeder>>();

        return services;
    }
}
