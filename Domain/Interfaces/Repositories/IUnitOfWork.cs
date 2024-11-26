using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Domain.Interfaces;

/// <summary>
/// Interface que define a unidade de trabalho (Unit of Work) para gerenciar os repositórios e operações no banco de dados.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Obtém o repositório genérico para a entidade especificada.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
    /// <returns>Repositório genérico para a entidade especificada.</returns>
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Salva as alterações no banco de dados de forma assíncrona.
    /// </summary>
    /// <returns>O número de registros afetados pela operação.</returns>
    Task<int> CommitAsync();

    /// <summary>
    /// Aplica o filtro para considerar apenas entidades ativas.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
    /// <param name="query">A consulta a ser filtrada.</param>
    /// <returns>A consulta filtrada.</returns>
    IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class;

    /// <summary>
    /// Propriedade para acessar o repositório de usuários.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Propriedade para acessar o repositório de roles.
    /// </summary>
    IRoleRepository Roles { get; }

    /// <summary>
    /// Propriedade para acessar o repositório de associações entre usuários e roles.
    /// </summary>
    IUserRoleRepository UserRoles { get; }

    /// <summary>
    /// Propriedade para acessar o repositório de permissões.
    /// </summary>
    IPermissionRepository Permissions { get; }

}
