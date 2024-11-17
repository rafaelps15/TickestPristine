using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : EntityBase
{
    protected readonly TickestContext _context;

    public BaseRepository(TickestContext context)
    {
        _context = context;
        _context.SaveChangesFailed += OnSaveChangesFailed;
    }

    #region CRUD Operations

    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await _context.Set<TEntity>().AsNoTracking().ToListAsync();

    public async Task<TEntity> GetByIdAsync(Guid id) =>
        await _context.Set<TEntity>().FindAsync(new object[] { id });

    public async Task AddAsync(TEntity entity, CancellationToken token = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
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
        if (typeof(TEntity).GetProperty("Description") == null)
            throw new InvalidOperationException("A entidade não possui uma propriedade 'Description'.");

        return await _context.Set<TEntity>()
                             .AsNoTracking()
                             .Where(entity => EF.Property<string>(entity, "Description").Contains(description))
                             .ToListAsync();
    }

    #endregion

    #region Handle Exception

    private void OnSaveChangesFailed(object sender, SaveChangesFailedEventArgs e)
    {
        //NÃO LANÇAR OUTRA EXCEPTION
        //PODE LOGGAR
    }

    #endregion
}
