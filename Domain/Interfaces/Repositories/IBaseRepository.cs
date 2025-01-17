using System.Linq.Expressions;
using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para operações básicas de repositório.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade.</typeparam>
public interface IBaseRepository<TEntity> : IDisposable where TEntity : EntityBase
{
    /// <summary>
    /// Encontra um registro que satisfaça a condição.
    /// </summary>
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se existe algum registro que satisfaça a condição.
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém todos os registros.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um registro pelo ID.
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona um registro.
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona múltiplos registros.
    /// </summary>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um registro.
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza múltiplos registros.
    /// </summary>
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deleta um registro pelo ID.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza a exclusão lógica de um registro.
    /// </summary>
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persiste as mudanças no banco de dados.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Libera os recursos.
    /// </summary>
    void Dispose();
}
