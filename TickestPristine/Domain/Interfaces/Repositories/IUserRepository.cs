using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken);
    Task<User?> GetByNameAsync(string userName);
    Task<User?> GetByIdWithDetailsAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> DoesEmailExistAsync(string userEmail, CancellationToken cancellationToken);
    Task<bool> AnyUsersExistAsync(CancellationToken cancellationToken);
}
