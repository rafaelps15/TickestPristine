using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    public TicketRepository(TickestContext context) : base(context) { }

    public async Task<IReadOnlyList<Ticket>> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken) =>
        await _context.Tickets
            .AsNoTracking()
            .Where(t => t.OpenedByUserId == userId && t.IsActive)
            .ToListAsync(cancellationToken);
}
