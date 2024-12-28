using Microsoft.EntityFrameworkCore.Storage;

namespace Tickest.Application.Abstractions.Services;

public interface ITransactionManager
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    Task RollbackAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
}
