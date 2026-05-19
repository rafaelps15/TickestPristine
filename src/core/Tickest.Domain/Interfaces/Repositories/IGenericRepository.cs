using System.Linq.Expressions;
using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}
