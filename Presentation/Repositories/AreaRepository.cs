using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class AreaRepository : BaseRepository<Area>, IAreaRepository
{
    public AreaRepository(TickestContext context) : base(context)
    {
    }

    public async Task<ICollection<Area>> GetByDescription(string description)
    {
        return await _context.Areas.Where(p => p.Description.Contains(description)).ToListAsync();
    }
}
