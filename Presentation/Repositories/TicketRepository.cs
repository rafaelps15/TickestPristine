using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Enum;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    protected readonly TickestContext _context;

    public TicketRepository(TickestContext context) : base(context) =>
        _context = context;

    // Obtém tickets de acordo com o status
    public async Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status)
    {
        var tickets = await _context.Tickets
            .Where(t => t.Status == status)
            .ToListAsync();

        // Returning a tuple with the tickets and the count
        return new Tuple<IEnumerable<Ticket>, int>(tickets, tickets.Count);
    }

    // Obtém todos os tickets de um usuário específico
    public async Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByUserIdAsync(Guid userId)
    {
        var tickets = await _context.Tickets
            .Where(t => t.UserId == userId)
            .ToListAsync();

        // Returning a tuple with the tickets and the count
        return new Tuple<IEnumerable<Ticket>, int>(tickets, tickets.Count);
    }

    // Verifica se um ticket existe
    public async Task<bool> TicketExistsAsync(Guid ticketId)
    {
        return await _context.Tickets
            .AnyAsync(t => t.Id == ticketId);
    }
}
