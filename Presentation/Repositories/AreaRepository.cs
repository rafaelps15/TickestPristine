using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class AreaRepository : GenericRepository<Area>, IAreaRepository
{
    protected readonly TickestContext _context;

    public AreaRepository(TickestContext context) : base(context) =>
        _context = context;
}
