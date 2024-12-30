using System.Linq.Expressions;
using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface genérica para repositórios de entidades, fornecendo métodos CRUD básicos.
/// </summary>
public interface IBaseRepository<TEntity> : IDisposable where TEntity : EntityBase
{
    /// <summary>
    /// Busca uma entidade com base em uma expressão de filtro.
    /// </summary>
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Retorna todas as entidades.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca uma entidade pelo ID.
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma entidade ao banco de dados.
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona várias entidades ao banco de dados.
    /// </summary>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade no banco de dados.
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza várias entidades no banco de dados.
    /// </summary>
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma entidade do banco de dados.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva as alterações no contexto de dados.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
