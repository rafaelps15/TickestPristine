using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Enum;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    protected readonly TickestContext _context;

    public TicketRepository(TickestContext context) : base(context) =>
        _context = context;

    #region Métodos de Consulta

    public async Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status)
    {
        var tickets = await _context.Tickets
            .Where(t => t.Status == status)
            .ToListAsync();

        return new Tuple<IEnumerable<Ticket>, int>(tickets, tickets.Count);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(Guid userId, CancellationToken cancellationToken) =>
        await _context.Tickets
            .Where(ticket => ticket.AssignedUserId == userId)
            .ToListAsync(cancellationToken);


    #endregion

    #region Métodos de Verificação

    public async Task<bool> TicketExistsAsync(Guid ticketId)=>
         await _context.Tickets
            .AnyAsync(t => t.Id == ticketId);
    

    #endregion
}
