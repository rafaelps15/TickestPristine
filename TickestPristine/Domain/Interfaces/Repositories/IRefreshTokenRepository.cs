using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken);

    Task<User> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
