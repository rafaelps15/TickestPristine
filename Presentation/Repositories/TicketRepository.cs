using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
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

    public async Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status)
    {
        var tickets = await _context.Tickets
            .Where(t => t.Status == status && t.IsActive)  // Filtra os tickets ativos
            .ToListAsync();

        return new Tuple<IEnumerable<Ticket>, int>(tickets, tickets.Count);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(Guid userId, CancellationToken cancellationToken) =>
        await _context.Tickets
            .Where(t => t.OpenedByUserId == userId && t.IsActive) // busca pelo usuario e verifica se está ativo
            .ToListAsync(cancellationToken);
        

    //public async Task<IEnumerable<Ticket>> GetTicketsByUserAndStatusAsync(Guid userId, TicketStatus status, CancellationToken cancellationToken)
    //{
    //    // Usando a tabela intermediária TicketUser para buscar tickets do usuário com status específico
    //    var tickets = await _context.Set<TicketUser>()
    //        .Where(tu => tu.UserId == userId && tu.Ticket.Status == status && tu.Ticket.IsActive)  // Filtra usuário, status e tickets ativos
    //        .Select(tu => tu.Ticket)  // Selecionando o Ticket relacionado ao User
    //        .ToListAsync(cancellationToken);

    //    return tickets;
    //}

    #endregion

    #region Métodos de Verificação

    public async Task<bool> TicketExistsAsync(Guid ticketId) =>
         await _context.Tickets
            .AnyAsync(t => t.Id == ticketId);

    //public async Task<bool> TicketUserExistsAsync(Guid ticketId, Guid userId)
    //{
    //    // Verificando se o Ticket está associado ao User na tabela intermediária TicketUser
    //    return await _context.Set<TicketUser>()
    //        .AnyAsync(tu => tu.TicketId == ticketId && tu.UserId == userId);
    //}

    #endregion
}
