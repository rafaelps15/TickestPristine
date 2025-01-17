using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tickest.Application.Abstractions.Data;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Data;

public class TickestContext : DbContext, IApplicationDbContext
{
    public TickestContext(DbContextOptions<TickestContext> options)
        : base(options) { }

    public DatabaseFacade Database => base.Database;


    // DbSets para cada entidade do sistema
    public DbSet<User> Users { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ApplicationSetting> ApplicationSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TickestContext).Assembly);
    }

}