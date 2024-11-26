using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SectorRepository : GenericRepository<Sector>, ISectorRepository
{
    protected readonly TickestContext _context;

    public SectorRepository(TickestContext context) : base(context) =>
        _context = context;

}
