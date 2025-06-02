using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(TickestContext context)
    {
        _context = context;
    }

    public Task BeginTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync().ContinueWith(t => _transaction = t.Result);
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task CommitTransactionAsync()
    {
        return _transaction.CommitAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
