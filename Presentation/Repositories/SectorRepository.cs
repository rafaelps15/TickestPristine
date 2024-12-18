using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SectorRepository : BaseRepository<Sector>,ISectorRepository
{
    protected readonly TickestContext _context;

    public SectorRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task<Sector> GetByIdWithDetailsAsync(Guid sectorId) =>
       await _context.Sectors
            .Include(s => s.Department)
            .Include(s => s.ResponsibleUser)
            .Include(s => s.Areas)
            .FirstOrDefaultAsync(s => s.Id == sectorId);
}
