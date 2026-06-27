using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Specifications;
using Tickest.Persistence.Data;
using Tickest.SharedKernel;

namespace Tickest.Persistence.Repositories;

public abstract class RepositoryBase<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : ValueObject
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryBase(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    protected abstract IQueryable<TEntity> GetBaseQuery();

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await DbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);

    public void Remove(TEntity entity) => DbSet.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);

    public virtual Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
        => GetBaseQuery().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<TEntity?> GetByIdAsync(
        TEntityId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> include,
        CancellationToken cancellationToken = default)
        => include(GetBaseQuery()).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<bool> ExistsAsync(TEntityId id, CancellationToken cancellationToken = default)
        => GetBaseQuery().AnyAsync(e => e.Id == id, cancellationToken);

    public Task<TEntity?> FindOneAsync(
        Specification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
        => ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);

    public Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default)
    {
        var query = include is null ? GetBaseQuery() : include(GetBaseQuery());
        return query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(
        Specification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
        => await ApplySpecification(specification).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default)
    {
        var query = include is null ? GetBaseQuery() : include(GetBaseQuery());
        return await query.Where(predicate).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(
        Specification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
        => ApplySpecification(specification).CountAsync(cancellationToken);

    public Task<bool> AnyAsync(
        Specification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
        => ApplySpecification(specification).AnyAsync(cancellationToken);

    private IQueryable<TEntity> ApplySpecification(Specification<TEntity, TEntityId> specification)
        => SpecificationEvaluator.GetQuery(GetBaseQuery(), specification);
}
