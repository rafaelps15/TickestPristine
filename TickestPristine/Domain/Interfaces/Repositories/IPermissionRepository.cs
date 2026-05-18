using Tickest.Domain.Entities.Permissions;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IPermissionRepository : IBaseRepository<Permission>
{
    //Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(Guid userId);

    Task<IEnumerable<Permission>> GetAllPermissionsAsync();

    Task<IEnumerable<Permission>> GetPermissionsByNamesAsync(IEnumerable<string> permissionNames, CancellationToken cancellationToken);
}
