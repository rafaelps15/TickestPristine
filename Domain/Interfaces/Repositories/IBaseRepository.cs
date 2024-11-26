using System.Linq.Expressions;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity>
{
    #region CRUD Operations

    /// <summary>
    /// Obtém todos os registros da entidade.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Obtém um registro da entidade por ID.
    /// </summary>
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona um novo registro à entidade.
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken token = default);

    /// <summary>
    /// Atualiza um registro existente na entidade.
    /// </summary>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Exclui um registro da entidade por ID.
    /// </summary>
    Task DeleteByIdAsync(Guid id);

    #endregion

    #region Custom Queries

    /// <summary>
    /// Busca registros com base em uma expressão predicada.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Busca registros com base em uma descrição.
    /// </summary>
    Task<IEnumerable<TEntity>> FindByDescriptionAsync(string description);

    #endregion
}
