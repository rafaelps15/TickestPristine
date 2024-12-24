using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;


public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
{
    private readonly TickestContext _context;

    public PermissionRepository(TickestContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Métodos de Consulta

    //public async Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(Guid userId) =>
    //         await _context.UserPermissions
    //        .Where(userPermission => userPermission.UserId == userId)
    //        .Select(userPermission => userPermission.Permission)
    //        .AsNoTracking()
    //        .ToListAsync();

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync() =>
             await _context.Permissions
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<Permission>> GetPermissionsByNamesAsync(IEnumerable<string> permissionNames, CancellationToken cancellationToken) =>
             await _context.Permissions
            .Where(p => permissionNames.Contains(p.Description))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    #endregion
}
