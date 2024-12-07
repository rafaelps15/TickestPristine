using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Domain.Interfaces;

/// <summary>
/// Interface que define os métodos de uma unidade de trabalho (UnitOfWork).
/// A unidade de trabalho é responsável por gerenciar as transações e repositórios.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositório de usuários.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Repositório de papéis (roles).
    /// </summary>
    //IRoleRepository Roles { get; }

    /// <summary>
    /// Repositório de relacionamentos entre usuários e papéis.
    /// </summary>
    //IUserRoleRepository UserRoles { get; }

    /// <summary>
    /// Repositório de tokens de atualização.
    /// </summary>
    IRefreshTokenRepository RefreshTokenRepository { get; }

    /// <summary>
    /// Repositório de tickets.
    /// </summary>
    ITicketRepository TicketRepository { get; }

    /// <summary>
    /// Retorna um repositório genérico para qualquer tipo de entidade.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <returns>O repositório genérico para o tipo da entidade.</returns>
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Salva as alterações feitas no contexto de dados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O número de registros afetados.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Aplica filtros nas consultas, como considerar apenas entidades ativas.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
    /// <param name="query">Consulta que será filtrada.</param>
    /// <returns>A consulta filtrada.</returns>
    IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class;
}
