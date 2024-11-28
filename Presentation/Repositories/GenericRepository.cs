using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly TickestContext _context;

    public GenericRepository(TickestContext context) =>
        _context = context;

    #region CRUD Operations

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    #endregion

    #region Custom Queries

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) =>
        await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public async Task<IEnumerable<TEntity>> FindByDescriptionAsync(string description, CancellationToken cancellationToken)
    {
        // Verifica se a entidade tem a propriedade "Description"
        if (typeof(TEntity).GetProperty("Description") == null)
            throw new TickestException("A entidade não possui uma propriedade 'Description'.");

        return await _context.Set<TEntity>()
                             .AsNoTracking()
                             .Where(entity => EF.Property<string>(entity, "Description").Contains(description))
                             .ToListAsync(cancellationToken);
    }

    #endregion
}
