using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SectorRepository : BaseRepository<Sector>,ISectorRepository
{
    protected readonly TickestContext _context;

    public SectorRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task AddDepartmentAsync(Department department, CancellationToken cancellationToken) =>
        await _context.Set<Department>().AddAsync(department, cancellationToken); 
}
