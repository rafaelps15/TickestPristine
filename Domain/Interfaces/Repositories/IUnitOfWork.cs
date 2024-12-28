using Microsoft.EntityFrameworkCore.Storage;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para o padrão Unit of Work, gerenciando repositórios e transações.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Obtém o repositório de usuários.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Obtém o repositório de tokens de atualização.
    /// </summary>
    IRefreshTokenRepository RefreshTokenRepository { get; }

    /// <summary>
    /// Obtém o repositório de tickets.
    /// </summary>
    ITicketRepository TicketRepository { get; }

    /// <summary>
    /// Obtém o repositório de áreas.
    /// </summary>
    IAreaRepository AreaRepository { get; }

    /// <summary>
    /// Obtém o repositório de especialidades.
    /// </summary>
    ISpecialtyRepository SpecialtyRepository { get; }

    /// <summary>
    /// Obtém o repositório de setores.
    /// </summary>
    ISectorRepository sectorRepository { get; }

    /// <summary>
    /// Obtém um repositório genérico para uma entidade específica.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <returns>O repositório genérico para a entidade.</returns>
    //IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Salva as alterações realizadas no contexto de banco de dados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>O número de registros afetados.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Executa um commit em lote para um conjunto de tarefas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <param name="batchTasks">As tarefas a serem executadas.</param>
    /// <returns>O número de registros afetados.</returns>
    Task<int> CommitBatchAsync(CancellationToken cancellationToken, IEnumerable<Task> batchTasks);

    /// <summary>
    /// Inicia uma nova transação no contexto do banco de dados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>A transação iniciada.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Confirma a transação atual.
    /// </summary>
    void CommitTransaction();

    /// <summary>
    /// Reverte a transação atual.
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    /// Aplica filtros de consulta em uma entidade.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="query">A consulta a ser filtrada.</param>
    /// <returns>A consulta filtrada.</returns>
    IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class;
}
