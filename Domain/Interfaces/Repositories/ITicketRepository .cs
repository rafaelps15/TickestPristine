using Tickest.Domain.Entities;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<Ticket> GetByIdAsync(object ticketId, CancellationToken cancellationToken);
    #region Custom Methods

    /// <summary>
    /// Obtém os tickets com base no status especificado.
    /// </summary>
    Task<Tuple<IEnumerable<Ticket>, int>> GetTicketsByStatusAsync(TicketStatus status);

    /// <summary>
    /// Recupera todos os tickets atribuídos a um usuário específico.
    /// </summary>
    /// <param name="userId">O identificador do usuário cujo tickets devem ser recuperados.</param>
    /// <param name="cancellationToken">O token de cancelamento para a operação assíncrona.</param>
    /// <returns>Uma coleção de tickets atribuídos ao usuário.</returns>
    Task<IEnumerable<Ticket>> GetTicketsByUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se um ticket existe com o ID fornecido.
    /// </summary>
    Task<bool> TicketExistsAsync(Guid ticketId);

    #endregion
}
