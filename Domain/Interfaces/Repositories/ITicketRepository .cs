using Tickest.Domain.Entities;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status);
    Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByUserIdAsync(Guid userId);
    Task<bool> TicketExistsAsync(Guid ticketId);
}
