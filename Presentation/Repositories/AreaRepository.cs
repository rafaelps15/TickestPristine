using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class AreaRepository : BaseRepository<Area>, IAreaRepository
{
    protected readonly TickestContext _context;

    public AreaRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task<IEnumerable<Area>> GetAreasByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken)
    {
        return await _context.Set<Area>()
            .Where(area => ids.Contains(area.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Area>> GetAvailableAsync(Department department, IEnumerable<Guid> areaIds, CancellationToken cancellationToken)
    {
        return await _context.Areas
            .Where(area => areaIds.Contains(area.Id) && !department.Areas.Any(dArea => dArea.Id == area.Id))
            .ToListAsync(cancellationToken);
    }

}
