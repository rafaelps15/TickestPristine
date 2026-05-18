using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SectorRepository : BaseRepository<Sector>, ISectorRepository
{
    public SectorRepository(TickestContext context) : base(context) { }
}
