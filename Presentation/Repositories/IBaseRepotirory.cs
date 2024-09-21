using Tickest.Domain.Entities;

namespace Tickest.Persistence.Repositories;

public interface IBaseRepotirory<TEntity> where TEntity : EntidadeBase
{
	Task<TEntity> GetByIdAsync(int id);

	Task<IEnumerable<TEntity>> GetAllAsync();

	Task AddAsync(TEntity entity);

	Task UpdateAsync(TEntity entity);

	Task DeleteByIdAsync(int id);
}
