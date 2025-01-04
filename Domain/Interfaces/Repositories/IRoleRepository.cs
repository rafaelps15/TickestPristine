using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken);
}
