using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal sealed class RoleRepository(ApplicationDbContext context)
    : Repository<Role>(context), IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == roleName && r.IsActive && !r.IsDeleted, cancellationToken);
    }
}
