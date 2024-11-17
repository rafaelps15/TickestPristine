using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    protected readonly TickestContext _context;

    public RoleRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task<Role> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(role => role.Name == name);
    }
}
