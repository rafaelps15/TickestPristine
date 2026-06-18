using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(TickestContext context) : base(context) { }

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == roleName && r.IsActive && !r.IsDeleted, cancellationToken);
    }
}
