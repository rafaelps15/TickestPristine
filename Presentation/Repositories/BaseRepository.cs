using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Entities.Base;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : EntityBase
{
    protected readonly TickestContext _context;

    public BaseRepository(TickestContext context) => _context = context;


    // Método genérico para verificar existência de registros
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
    }

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.Now;
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }

        await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdateAt = DateTime.Now;
        _context.Set<TEntity>().Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.UpdateAt = DateTime.UtcNow;
        }

        _context.Set<TEntity>().UpdateRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            _context.Set<TEntity>().Remove(entity);
            await SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        entity.SoftDelete();
        _context.Set<TEntity>().Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
