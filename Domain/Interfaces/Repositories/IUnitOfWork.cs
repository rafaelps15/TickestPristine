using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Domain.Interfaces;

/// <summary>
/// Interface que define os métodos de uma unidade de trabalho (UnitOfWork).
/// A unidade de trabalho é responsável por gerenciar as transações e repositórios, garantindo que as operações no banco de dados sejam executadas de forma consistente.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositório de usuários. Permite realizar operações relacionadas à entidade de usuários.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Repositório de tokens de atualização. Permite gerenciar os tokens de atualização utilizados na autenticação.
    /// </summary>
    IRefreshTokenRepository RefreshTokenRepository { get; }

    /// <summary>
    /// Repositório de tickets. Permite gerenciar as operações relacionadas aos tickets.
    /// </summary>
    ITicketRepository TicketRepository { get; }

    /// <summary>
    /// Repositório de especialidades. Permite gerenciar as operações relacionadas às especialidades.
    /// </summary>
    ISpecialtyRepository SpecialtyRepository { get; }

    /// <summary>
    /// Repositório de áreas. Permite gerenciar as operações relacionadas às áreas.
    /// </summary>
    IAreaRepository AreaRepository { get; }

    /// <summary>
    /// Retorna um repositório genérico para qualquer tipo de entidade.
    /// Esse método permite acessar repositórios para entidades que não têm um repositório específico já definido.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade para o qual o repositório será retornado.</typeparam>
    /// <returns>Um repositório genérico para o tipo da entidade especificada.</returns>
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Salva as alterações feitas no contexto de dados.
    /// Este método persiste todas as modificações realizadas nas entidades do contexto, garantindo a consistência dos dados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento, que pode ser utilizado para interromper a operação.</param>
    /// <returns>O número de registros afetados pela operação de persistência.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Aplica filtros nas consultas, como considerar apenas entidades ativas ou outras condições.
    /// Esse método pode ser utilizado para garantir que apenas dados válidos ou relevantes sejam consultados.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade que será filtrada.</typeparam>
    /// <param name="query">A consulta que será filtrada.</param>
    /// <returns>A consulta filtrada com base nas condições especificadas.</returns>
    IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class;
}
