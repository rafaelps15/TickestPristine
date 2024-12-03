using Tickest.Domain.Common;
using Tickest.Domain.Entities;

namespace Tickest.Application.Abstractions.Authentication;

public interface IAuthService
{
    /// <summary>
    /// Autentica o usuário e retorna um token JWT.
    /// </summary>
    //Task<TokenResponse> AuthenticateAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o usuário atual a partir do contexto HTTP autenticado.
    /// </summary>
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Renova o token JWT utilizando o refresh token fornecido.
    /// </summary>
    Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o usuário associado ao refresh token fornecido.
    /// </summary>
    Task<User> GetUserByRefreshTokenAsync(string refreshToken);
}
