using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Domain.Interfaces;

/// <summary>
/// Representa a unidade de trabalho, fornecendo acesso a repositórios e métodos para salvar alterações no banco de dados.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Obtém o repositório de usuários.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Obtém o repositório de roles.
    /// </summary>
    IRoleRepository Roles { get; }

    /// <summary>
    /// Obtém o repositório de user roles.
    /// </summary>
    IUserRoleRepository UserRoles { get; }

    /// <summary>
    /// Obtém o repositório genérico para a entidade especificada.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <returns>Repositório genérico para a entidade especificada.</returns>
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Salva as alterações no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
    /// <returns>O número de registros afetados.</returns>
    /// <exception cref="TickestException">Lançado em caso de erro durante o commit da transação.</exception>
    Task<int> CommitAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Aplica o filtro para considerar apenas entidades ativas.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="query">A consulta que será filtrada.</param>
    /// <returns>A consulta filtrada para retornar apenas entidades ativas.</returns>
    IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class;
}
