using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Sector> Sectors { get; }
    DbSet<Department> Departments { get; }
    DbSet<Specialty> Specialties { get; }
    DbSet<UserSpecialty> UserSpecialties { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<Ticket> Tickets { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Role> Roles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
