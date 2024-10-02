using Tickest.Domain.Entities;

namespace Tickest.Domain.Repositories;

public interface IBaseRepository<TEntity> where TEntity : EntidadeBase
{
    Task<TEntity> GetByIdAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity);

    Task DeleteByIdAsync(int id);
}
