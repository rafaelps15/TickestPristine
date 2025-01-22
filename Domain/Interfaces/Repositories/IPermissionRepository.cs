using Tickest.Domain.Entities.Permissions;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IPermissionRepository: IBaseRepository<Permission>
{
    Task<Permission> GetPermissionByNameAsync(string name);
}
