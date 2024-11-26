using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Data;

public class TickestContext : DbContext
{
    public TickestContext(DbContextOptions<TickestContext> options)
        : base(options) { }

    // DbSets para cada entidade do sistema
    public DbSet<User> Users { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<UserSpecialty> UserSpecialties { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketUser> TicketUsers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserPermission> UserPermissions { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TickestContext).Assembly);
    }

}