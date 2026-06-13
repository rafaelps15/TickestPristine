using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(TickestContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .Include(user => user.Role)
                      .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<User?> GetByEmployeeCodeAsync(string employeeCode, CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode, cancellationToken);

    public async Task<User?> GetWithPermissionsAsync(Guid userId, CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .Include(user => user.UserSpecialties)
                          .ThenInclude(userSpecialty => userSpecialty.Specialty)
                      .Include(user => user.Role)
                      .Include(user => user.Permissions)
                      .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

    public async Task<bool> AnyAsync(CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .AnyAsync(cancellationToken);
}
