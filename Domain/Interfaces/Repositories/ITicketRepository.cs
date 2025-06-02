using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para repositório de tickets, fornecendo métodos para consultar, verificar e manipular tickets.
/// </summary>
public interface ITicketRepository:IBaseRepository<Ticket, Guid>
{
    /// <summary>
    /// Obtém uma lista de tickets com um status específico e ativos.
    /// </summary>
    /// <param name="status">O status dos tickets a serem recuperados.</param>
    /// <returns>Uma lista de tickets com o status especificado e ativos.</returns>
    Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(TicketStatus status);

    /// <summary>
    /// Obtém uma lista de tickets associados a um usuário específico e ativos.
    /// </summary>
    /// <param name="userId">O identificador único do usuário.</param>
    /// <param name="cancellationToken">O token de cancelamento para controle de operações assíncronas.</param>
    /// <returns>Uma lista de tickets associados ao usuário e ativos.</returns>
    Task<IEnumerable<Ticket>> GetTicketsByUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se um ticket existe no repositório com base no ID fornecido.
    /// </summary>
    /// <param name="ticketId">O identificador único do ticket.</param>
    /// <returns>Um valor booleano indicando se o ticket existe ou não.</returns>
    Task<bool> TicketExistsAsync(Guid ticketId);
}
