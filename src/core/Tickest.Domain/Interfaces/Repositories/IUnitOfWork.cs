namespace Tickest.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    ITicketRepository TicketRepository { get; }
    IAreaRepository AreaRepository { get; }
    ISpecialtyRepository SpecialtyRepository { get; }
    ISectorRepository SectorRepository { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task<int> CommitAsync(CancellationToken cancellationToken);
    Task<int> CommitBatchAsync(CancellationToken cancellationToken, IEnumerable<Task> batchTasks);
    void CommitTransaction();
    void RollbackTransaction();
}
