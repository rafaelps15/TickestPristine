using Tickest.Domain.Entities.Tickets;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<IReadOnlyList<Ticket>> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken);
}
