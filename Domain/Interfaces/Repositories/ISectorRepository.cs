using Tickest.Domain.Entities.Sectors;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ISectorRepository : IBaseRepository<Sector>
{
    Task AddDepartmentAsync(Department department, CancellationToken cancellationToken);
    //Task UpdateAsync(Department department, CancellationToken cancellationToken);
}
