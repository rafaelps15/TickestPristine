using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Data
{
    public class TickestContext : DbContext
    {
        public TickestContext(DbContextOptions<TickestContext> options)
            : base(options) { }

        // DbSets para cada entidade do sistema
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}