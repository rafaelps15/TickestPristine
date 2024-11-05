using System.Threading;
using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : EntityBase
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
}
