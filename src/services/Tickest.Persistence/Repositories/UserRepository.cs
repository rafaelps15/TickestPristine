using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext context)
    : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await DbSet
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<User?> GetByEmployeeCodeAsync(string employeeCode, CancellationToken cancellationToken) =>
        await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode, cancellationToken);

    public async Task<User?> GetWithPermissionsAsync(Guid userId, CancellationToken cancellationToken) =>
        await DbSet
            .AsNoTracking()
            .Include(u => u.UserSpecialties)
                .ThenInclude(us => us.Specialty)
            .Include(u => u.Role)
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

    public async Task<bool> AnyAsync(CancellationToken cancellationToken) =>
        await DbSet
            .AsNoTracking()
            .AnyAsync(cancellationToken);
}
