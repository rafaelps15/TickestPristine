﻿using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Security;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Data;

public class TickestContext : DbContext
{
    public TickestContext(DbContextOptions<TickestContext> options)
        : base(options) { }

    // DbSets para cada entidade do sistema
    public DbSet<User> Users { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<UserSpecialty> UserSpecialties { get; set; }
    //public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    //public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    //public DbSet<TicketUser> TicketUsers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    //public DbSet<UserRole> UserRoles { get; set; }
    //public DbSet<UserPermission> UserPermissions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TickestContext).Assembly);
    }

}