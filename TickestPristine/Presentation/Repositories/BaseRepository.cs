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

    /// <summary>
    /// O método <c>AsNoTracking</c> do Entity Framework Core é utilizado para realizar consultas onde o rastreamento das entidades no contexto não é necessário. 
    /// Isso significa que o Entity Framework não irá acompanhar as mudanças feitas nas entidades recuperadas, o que pode melhorar o desempenho 
    /// em cenários onde você só precisa ler dados e não precisa atualizar ou deletar as entidades.
    /// </summary>
    /// <remarks>
    /// Quando você usa <c>AsNoTracking</c>, o Entity Framework não irá armazenar informações sobre as entidades no contexto, 
    /// o que reduz o uso de memória e melhora o desempenho das consultas. Essa prática é particularmente útil em operações de leitura, 
    /// como quando você exibe dados em uma página ou realiza consultas para relatórios. 
    /// Contudo, se você precisar atualizar ou excluir entidades, o Entity Framework não poderá rastrear essas mudanças, 
    /// e será necessário chamar explicitamente <c>Attach</c> ou outros métodos para reanexar a entidade ao contexto.
    /// </remarks>

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ? _context.Set<TEntity>().AsNoTracking() : _context.Set<TEntity>();
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ? _context.Set<TEntity>().AsNoTracking() : _context.Set<TEntity>();
        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, false, cancellationToken);
        if (entity is not null)
        {
            _context.Set<TEntity>().Remove(entity);
            await SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    protected async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

}
