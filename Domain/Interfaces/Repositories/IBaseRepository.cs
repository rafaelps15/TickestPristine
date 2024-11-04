using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity> GetByIdAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
}
