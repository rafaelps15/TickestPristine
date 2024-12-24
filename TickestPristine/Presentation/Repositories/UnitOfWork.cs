using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tickest.Application.Abstractions.Services;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TickestContext _context;
    private readonly IQueryFilterService _queryFilterService;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly ISectorRepository _sectorRepository;
    private bool _disposed;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(
        TickestContext context,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITicketRepository ticketRepository,
        ISpecialtyRepository specialtyRepository,
        IAreaRepository areaRepository,
        IQueryFilterService queryFilterService)
        => (_context, _queryFilterService, _userRepository, _refreshTokenRepository, _ticketRepository, _specialtyRepository, _areaRepository, _sectorRepository ) =
        (context, queryFilterService, userRepository, refreshTokenRepository, ticketRepository, specialtyRepository, areaRepository, sectorRepository );

    public IUserRepository Users => _userRepository;
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
    public ITicketRepository TicketRepository => _ticketRepository;
    public IAreaRepository AreaRepository => _areaRepository;
    public ISpecialtyRepository SpecialtyRepository => _specialtyRepository;
    public ISectorRepository sectorRepository => _sectorRepository;

    public IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class =>
        _queryFilterService.ApplyFilters(query);

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_currentTransaction == null)
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                await _currentTransaction.CommitAsync(cancellationToken);
                return 1;
            }
        }
        catch (DbUpdateException dbEx)
        {
            await RollbackCurrentTransaction(cancellationToken);
            throw new TickestException("Erro ao tentar salvar as alterações no banco de dados.", dbEx);
        }
        catch (Exception ex)
        {
            await RollbackCurrentTransaction(cancellationToken);
            throw new TickestException("Ocorreu um erro ao processar a transação.", ex);
        }
    }

    private async Task RollbackCurrentTransaction(CancellationToken cancellationToken)
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
    }

    public async Task<int> CommitBatchAsync(CancellationToken cancellationToken, IEnumerable<Task> batchTasks)
    {
        try
        {
            await Task.WhenAll(batchTasks);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new TickestException("Erro ao realizar commit em lote.", ex);
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction;
    }

    public void CommitTransaction()
    {
        _currentTransaction?.Commit();
    }

    public void RollbackTransaction()
    {
        _currentTransaction?.Rollback();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
}
