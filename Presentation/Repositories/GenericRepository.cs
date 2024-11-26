using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Entities; 
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
{
    protected readonly TickestContext _context;

    public GenericRepository(TickestContext context) =>
        _context = context;

    #region CRUD Operations

    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await _context.Set<TEntity>().AsNoTracking().ToListAsync();

    public async Task<TEntity> GetByIdAsync(Guid id) =>
        await _context.Set<TEntity>().FindAsync(id);

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    #endregion

    #region Custom Queries

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();

    public async Task<IEnumerable<TEntity>> FindByDescriptionAsync(string description)
    {
        // Verifica se a entidade tem a propriedade "Description"
        if (typeof(TEntity).GetProperty("Description") == null)
            throw new InvalidOperationException("A entidade não possui uma propriedade 'Description'.");

        return await _context.Set<TEntity>()
                             .AsNoTracking()
                             .Where(entity => EF.Property<string>(entity, "Description").Contains(description))
                             .ToListAsync();
    }

    #endregion
}
