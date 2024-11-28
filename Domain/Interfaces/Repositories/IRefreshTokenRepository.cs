using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    #region Custom Methods

    /// <summary>
    /// Obtém o RefreshToken com base no token fornecido.
    /// </summary>
    Task<RefreshToken> GetByTokenAsync(string token);

    /// <summary>
    /// Obtém o usuário associado ao RefreshToken fornecido.
    /// </summary>
    Task<User> GetByRefreshTokenAsync(string refresToken);

    #endregion
}
