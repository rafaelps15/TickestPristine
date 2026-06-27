using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly ISectorRepository _sectorRepository;
    private bool _disposed;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(
        ApplicationDbContext context,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITicketRepository ticketRepository,
        ISpecialtyRepository specialtyRepository,
        ISectorRepository sectorRepository)
        => (_context, _userRepository, _refreshTokenRepository, _ticketRepository, _specialtyRepository, _sectorRepository) =
           (context, userRepository, refreshTokenRepository, ticketRepository, specialtyRepository, sectorRepository);

    public IUserRepository Users => _userRepository;
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
    public ITicketRepository TicketRepository => _ticketRepository;
    public ISpecialtyRepository SpecialtyRepository => _specialtyRepository;
    public ISectorRepository SectorRepository => _sectorRepository;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_currentTransaction is null)
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
            return result;
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

    public void CommitTransaction()
    {
        _currentTransaction?.Commit();
    }

    public void RollbackTransaction()
    {
        _currentTransaction?.Rollback();
    }

    private async Task RollbackCurrentTransaction(CancellationToken cancellationToken)
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _currentTransaction?.Dispose();
            _context.Dispose();
        }

        _disposed = true;
    }
}
