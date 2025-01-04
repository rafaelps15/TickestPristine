using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Enum;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    protected readonly TickestContext _context;

    public TicketRepository(TickestContext context) : base(context) =>
        _context = context;

    #region Métodos de Consulta

    public async Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(TicketStatus status) =>
        // Retorna a lista de tickets com o status e ativos
        await _context.Tickets
            .Where(t => t.Status == status && t.IsActive)
            .ToListAsync();

    public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(Guid userId, CancellationToken cancellationToken) =>
        // Retorna a lista de tickets do usuário com o estado ativo
        await _context.Tickets
            .Where(t => t.OpenedByUserId == userId && t.IsActive)
            .ToListAsync(cancellationToken);

    #endregion

    #region Métodos de Verificação

    public async Task<bool> TicketExistsAsync(Guid ticketId) =>
        // Verifica se o ticket com o Id informado existe
        await _context.Tickets
            .AnyAsync(t => t.Id == ticketId);

    #endregion
}
