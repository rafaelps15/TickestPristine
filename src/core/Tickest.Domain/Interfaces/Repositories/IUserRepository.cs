using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetWithPermissionsAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> AnyAsync(CancellationToken cancellationToken);
}
