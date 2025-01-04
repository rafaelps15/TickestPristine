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

    //public async Task<List<Area>> GetAllAreasAsync(CancellationToken cancellationToken)
    //{
    //    return await _context.Areas
    //        .AsNoTracking()
    //        .Include(a => a.Department)
    //        .Include(a => a.Users)
    //        .ToListAsync(cancellationToken);
    //}
}
