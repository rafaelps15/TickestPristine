using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;

namespace Tickest.Persistence.Repositories;

internal class BaseRepository<TEntity> : IBaseRepotirory<TEntity> where TEntity : EntidadeBase
{
    protected readonly TickestContext _context;

    public BaseRepository(TickestContext context)
    {
        _context=context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
