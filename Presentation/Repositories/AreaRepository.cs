using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class AreaRepository : BaseRepository<Area>, IAreaRepository
{
    protected readonly TickestContext _context;

    public AreaRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task<List<Area>> GetAreasWithSpecialtiesByIdsAsync(List<Guid> areaIds, CancellationToken cancellationToken)
    {
        return await _context.Areas
            .Include(area => area.Specialty)
            .Where(area => areaIds.Contains(area.Id))
            .ToListAsync(cancellationToken);
    }
}
