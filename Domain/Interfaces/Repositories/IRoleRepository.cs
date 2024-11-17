using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IRoleRepository: IBaseRepository<Role>
{
    Task<Role> GetByNameAsync(string name);
}
