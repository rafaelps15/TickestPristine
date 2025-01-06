using Tickest.Domain.Entities.Sectors;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ISectorRepository : IBaseRepository<Sector>
{
    //Task AddAsync(Sector sector, CancellationToken cancellationToken);
}
