using Tickest.Domain.Entities;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    #region Custom Methods

    /// <summary>
    /// Obtém os tickets com base no status especificado.
    /// </summary>
    Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status);

    /// <summary>
    /// Obtém todos os tickets de um usuário específico.
    /// </summary>
    Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByUserIdAsync(Guid userId);

    /// <summary>
    /// Verifica se um ticket existe com o ID fornecido.
    /// </summary>
    Task<bool> TicketExistsAsync(Guid ticketId);

    #endregion
}
