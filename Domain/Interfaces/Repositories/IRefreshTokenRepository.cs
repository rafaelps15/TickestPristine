using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
        Task<User> GetByRefreshTokenAsync(string refresToken);
    }
}

