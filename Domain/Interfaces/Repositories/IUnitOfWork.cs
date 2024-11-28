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
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Salva as alterações no banco de dados de forma assíncrona.
    /// </summary>
    /// <returns>O número de registros afetados.</returns>
    Task<int> CommitAsync();

    /// <summary>
    /// Aplica o filtro para considerar apenas entidades ativas.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="query">A consulta que será filtrada.</param>
    /// <returns>A consulta filtrada.</returns>
    IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class;
}
