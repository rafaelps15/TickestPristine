using System.Linq.Expressions;

namespace Tickest.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        #region CRUD Operations

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Guid id);
        Task AddAsync(TEntity entity, CancellationToken token = default);
        Task UpdateAsync(TEntity entity);
        Task DeleteByIdAsync(Guid id);

        #endregion

        #region Custom Queries

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindByDescriptionAsync(string description);

        #endregion
    }
}
