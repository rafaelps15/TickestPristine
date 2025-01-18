using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
{
    protected readonly TickestContext _context;

    public PermissionRepository(TickestContext context) : base(context)
        => _context = context;

    public async Task<Permission> GetPermissionByNameAsync(string permissionName, CancellationToken cancellationToken) =>
        await _context.Permissions
             .FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken);

}
