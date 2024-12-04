using Tickest.Application.DTOs;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication;

public interface IAuthService
{
    /// <summary>
    /// Autentica o usuário com base no e-mail e na senha fornecidos, retornando um token de acesso.
    /// </summary>
    /// <param name="email">O e-mail do usuário.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <returns>Um <see cref="TokenResponse"/> contendo o token de acesso e informações sobre a expiração.</returns>
    Task<TokenResponse> AuthenticateAsync(string email, string password);

    /// <summary>
    /// Obtém o usuário atualmente autenticado.
    /// </summary>
    /// <returns>O <see cref="User"/> atual.</returns>
    /// <exception cref="TickestException">Lançado se o usuário não for encontrado ou não estiver autenticado.</exception>
    Task<User> GetCurrentUserAsync();

    /// <summary>
    /// Renova o token JWT utilizando o refresh token fornecido.
    /// </summary>
    Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o usuário associado ao refresh token fornecido.
    /// </summary>
    Task<User> GetUserByRefreshTokenAsync(string refreshToken);
}
