using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Data;

public class TickestContext : DbContext
{
    public TickestContext(DbContextOptions<TickestContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TickestContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Area> Areas { get; set; }
    public DbSet<SupportTicket> SupportTickets { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}
