using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para o repositório de Tickets.
/// Define métodos para acesso aos dados relacionados a Tickets.
/// </summary>
public interface ITicketRepository : IBaseRepository<Ticket>
{
    /// <summary>
    /// Obtém todos os tickets de acordo com o status informado.
    /// </summary>
    /// <param name="status">O status do ticket a ser filtrado.</param>
    /// <returns>Uma tupla contendo uma lista de tickets e o total de tickets encontrados.</returns>
    Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status);

    /// <summary>
    /// Obtém todos os tickets de um usuário específico.
    /// </summary>
    /// <param name="userId">O ID do usuário para filtrar os tickets associados.</param>
    /// <param name="cancellationToken">O token de cancelamento para a operação assíncrona.</param>
    /// <returns>Uma lista de tickets associados ao usuário informado.</returns>
    Task<IEnumerable<Ticket>> GetTicketsByUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém todos os tickets de um usuário com um status específico.
    /// </summary>
    /// <param name="userId">O ID do usuário para filtrar os tickets associados.</param>
    /// <param name="status">O status do ticket a ser filtrado.</param>
    /// <param name="cancellationToken">O token de cancelamento para a operação assíncrona.</param>
    /// <returns>Uma lista de tickets associados ao usuário com o status informado.</returns>
    //Task<IEnumerable<Ticket>> GetTicketsByUserAndStatusAsync(Guid userId, TicketStatus status, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se o ticket existe no banco de dados.
    /// </summary>
    /// <param name="ticketId">O ID do ticket a ser verificado.</param>
    /// <returns>True se o ticket existir, caso contrário, false.</returns>
    Task<bool> TicketExistsAsync(Guid ticketId);

    /// <summary>
    /// Verifica se um ticket específico está associado a um usuário.
    /// </summary>
    /// <param name="ticketId">O ID do ticket a ser verificado.</param>
    /// <param name="userId">O ID do usuário a ser verificado.</param>
    /// <returns>True se o ticket estiver associado ao usuário, caso contrário, false.</returns>
    //Task<bool> TicketUserExistsAsync(Guid ticketId, Guid userId);
}
