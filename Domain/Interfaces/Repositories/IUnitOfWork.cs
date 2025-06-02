using Microsoft.EntityFrameworkCore.Storage;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para o padrão Unit of Work, gerenciando repositórios e transações.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync();
}

