using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;


public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    private readonly TickestContext _context;

    public PermissionRepository(TickestContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Métodos de Consulta

    public async Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("O ID do usuário não pode ser vazio.", nameof(userId));
        }

        return await _context.UserPermissions
            .Where(userPermission => userPermission.UserId == userId)
            .Select(userPermission => userPermission.Permission)
            .AsNoTracking()
            .ToListAsync();
    }

    
    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permissions
            .AsNoTracking()
            .ToListAsync();
    }

    #endregion
}
