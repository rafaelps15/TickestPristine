using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status);

    Task<IEnumerable<Ticket>> GetTicketsByUserAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool> TicketExistsAsync(Guid ticketId);
}
