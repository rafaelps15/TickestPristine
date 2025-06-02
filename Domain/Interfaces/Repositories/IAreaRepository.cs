using Tickest.Domain.Entities.Sectors;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IAreaRepository : IBaseRepository<Area,Guid>
{
    Task<IEnumerable<Area>> GetAreasByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken);
    Task<IEnumerable<Area>> GetAvailableAsync(Department department, IEnumerable<Guid> areaIds, CancellationToken cancellationToken);
}
