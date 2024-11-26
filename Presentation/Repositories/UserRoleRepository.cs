using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    private readonly TickestContext _context;

    public UserRoleRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId) =>
        await _context.UserRoles
                      .Where(ur => ur.UserId == userId)
                      .Include(ur => ur.Role)
                      .ToListAsync();

    public async Task<bool> HasRoleAsync(Guid userId, Guid roleId) =>
        await _context.UserRoles
                      .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);


    public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdsAsync(IEnumerable<Guid> userIds) =>
        await _context.UserRoles
                      .Where(ur => userIds.Contains(ur.UserId))
                      .Include(ur => ur.Role)
                      .ToListAsync();
}
