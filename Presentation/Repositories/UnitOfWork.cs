using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly TickestContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IGenericRepository<TickestContext> _genericRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IAreaRepository _areaRepository;
    private bool _disposed;

    public UnitOfWork(
        TickestContext context,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITicketRepository ticketRepository,
        ISpecialtyRepository specialtyRepository,
        IAreaRepository areaRepository,
        IGenericRepository<TickestContext> genericRepository) =>
        (_context, _userRepository, _refreshTokenRepository, _ticketRepository, _specialtyRepository, _areaRepository, _genericRepository) =
        (context, userRepository, refreshTokenRepository, ticketRepository, specialtyRepository, areaRepository, genericRepository);

    #region - Métodos Públicos

    public IUserRepository Users => _userRepository;
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
    public ITicketRepository TicketRepository => _ticketRepository;
    public IAreaRepository AreaRepository => _areaRepository;
    public ISpecialtyRepository SpecialtyRepository => _specialtyRepository;

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class =>
        (IGenericRepository<TEntity>)_genericRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException dbEx)
        {
            throw new TickestException("Erro ao tentar salvar as alterações no banco de dados.", dbEx);
        }
        catch (Exception ex)
        {
            throw new TickestException("Ocorreu um erro ao processar a transação.", ex);
        }
    }

    /// <summary>
    /// Aplica o filtro para considerar apenas entidades ativas.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="query">A consulta que será filtrada.</param>
    /// <returns>A consulta filtrada para retornar apenas entidades ativas.</returns>
    public IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        // Exemplo de filtro que pode ser aplicado
        return query.Where(entity => EF.Property<bool>(entity, "IsActive"));
    }

    /// <summary>
    /// Libera os recursos utilizados pela unidade de trabalho.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Libera os recursos utilizados pela unidade de trabalho.
    /// </summary>
    /// <param name="disposing">Indica se o método foi chamado pelo Dispose() ou pelo finalizador.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    #endregion
}
