using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ISectorRepository : IBaseRepository<Sector>
{
    Task AddDepartmentAsync(Department department, CancellationToken cancellationToken);

    Task<User> GetSectorManagerByIdAsync(Guid sectorManagerId, CancellationToken cancellationToken);
}
