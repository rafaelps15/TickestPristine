using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IRoleRepository : IBaseRepository<Role, Guid>
{
    Task<Role> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken);
}
