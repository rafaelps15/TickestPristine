using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class RoleRepository : BaseRepository<Role, Guid>, IRoleRepository
{
    protected readonly TickestContext _context;

    public RoleRepository(TickestContext context) : base(context)
        => _context = context;

    #region Métodos de consulta

    public async Task<Role> GetByNameAsync(string roleName, CancellationToken cancellationToken) =>
        await _context.Set<Role>()
           .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);


    public async Task<bool> AnyRoleExistsAsync(Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>().FirstOrDefaultAsync(predicate, cancellationToken);
        return role != null;
    }

 

    #endregion
}
