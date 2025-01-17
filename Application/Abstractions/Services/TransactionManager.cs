//using Microsoft.EntityFrameworkCore.Storage;
//using Tickest.Application.Abstractions.Data;

//namespace Tickest.Application.Abstractions.Services;

//public class TransactionManager : ITransactionManager
//{
//    private readonly IApplicationDbContext _context;

//    public TransactionManager(IApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
//    {
//        return await _context.Database.BeginTransactionAsync(cancellationToken);
//    }

//    public async Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
//    {
//        await transaction.CommitAsync(cancellationToken);
//    }

//    public async Task RollbackAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
//    {
//        await transaction.RollbackAsync(cancellationToken);
//    }
//}
