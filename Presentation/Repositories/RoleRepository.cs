using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    protected readonly TickestContext _context;

    public RoleRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task<Role> GetByNameAsync(string name) =>
        await _context.Roles
            .FirstOrDefaultAsync(role => role.Name == name);
    

    public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId) =>
        await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .ToListAsync();

    public async Task<IEnumerable<Role>> GetRolesByNamesAsync(IEnumerable<string> roleNames) =>
        await _context.Roles
            .Where(role => roleNames.Contains(role.Name))
            .ToListAsync();
}
