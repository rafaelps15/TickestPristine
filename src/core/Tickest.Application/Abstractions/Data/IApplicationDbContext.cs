using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
