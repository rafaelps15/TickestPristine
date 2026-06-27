using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal sealed class TicketRepository(ApplicationDbContext context)
    : Repository<Ticket>(context), ITicketRepository
{
    public async Task<IReadOnlyList<Ticket>> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken) =>
        await DbSet
            .AsNoTracking()
            .Where(t => t.OpenedByUserId == userId && t.IsActive)
            .ToListAsync(cancellationToken);
}
