using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para o repositório de Refresh Tokens.
/// Define os métodos necessários para manipulação e consulta de tokens de refresh.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Obtém um RefreshToken pelo valor do token.
    /// </summary>
    /// <param name="token">O valor do token a ser consultado.</param>
    /// <returns>Um objeto <see cref="RefreshToken"/> correspondente ao token fornecido.</returns>
    Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o usuário associado a um RefreshToken.
    /// </summary>
    /// <param name="refreshToken">O token de refresh a ser utilizado para consultar o usuário.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação assíncrona.</param>
    /// <returns>O <see cref="User"/> associado ao refresh token.</returns>
    /// <exception cref="TickestException">Lança uma exceção caso o refresh token ou o usuário não sejam encontrados.</exception>
    Task<User> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
