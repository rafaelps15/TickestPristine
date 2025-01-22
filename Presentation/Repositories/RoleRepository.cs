using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    protected readonly TickestContext _context;

    public RoleRepository(TickestContext context) : base(context)
        => _context = context;

    #region Métodos de consulta

    public async Task<Role> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken) =>
        await _context.Set<Role>()
           .Where(r => r.Name == roleName)
           .FirstOrDefaultAsync(cancellationToken);
       

    public async Task<bool> AnyRoleExistsAsync(Expression<Func<Role,bool>> predicate,CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>().FirstOrDefaultAsync(predicate, cancellationToken);
        return role != null;
    }

    #endregion
}
