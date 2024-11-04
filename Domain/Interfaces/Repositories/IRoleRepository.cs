using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(int id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(int id);
}
