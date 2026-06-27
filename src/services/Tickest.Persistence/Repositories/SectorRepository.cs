using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SectorRepository : Repository<Sector>, ISectorRepository
{
    public SectorRepository(ApplicationDbContext context) : base(context) { }
}
