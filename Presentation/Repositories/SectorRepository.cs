using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SectorRepository : BaseRepository<Sector,Guid>, ISectorRepository
{
    protected readonly TickestContext _context;

    public SectorRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task AddDepartmentAsync(Department department, CancellationToken cancellationToken) =>
        await _context.Set<Department>().AddAsync(department, cancellationToken);

    public async Task<User> GetSectorManagerByIdAsync(Guid sectorManagerId, CancellationToken cancellationToken) =>
        await _context.Users
            .FirstOrDefaultAsync(u => u.Id == sectorManagerId, cancellationToken);

   
}
