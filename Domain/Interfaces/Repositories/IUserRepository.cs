using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;


public interface IUserRepository : IBaseRepository<User>
{

    Task<User> GetUserByEmailAsync(string userEmail);
    Task<bool> DoesEmailExistAsync(string userEmail);
}
