using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Entities.Base;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.SharedKernel;

namespace Tickest.Persistence.Repositories;

public abstract class Repository<TEntity, TEntityId> : RepositoryBase<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : ValueObject
{
    protected Repository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<TEntity> GetBaseQuery() => DbSet.AsQueryable();

    public override Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
        => DbSet.FindAsync([id], cancellationToken).AsTask();
}

public class Repository<TEntity> : Repository<TEntity, EntityId>, IBaseRepository<TEntity>
    where TEntity : AuditableEntity
{
    protected Repository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return FindOneAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ? GetBaseQuery().AsNoTracking() : GetBaseQuery();
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            return await GetByIdAsync((EntityId)id, cancellationToken);
        }

        return await GetBaseQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, false, cancellationToken);

        if (entity is not null)
        {
            Remove(entity);
        }
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        RemoveRange(entities);
        return Task.CompletedTask;
    }
}
