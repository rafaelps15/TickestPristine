using Tickest.Application.DTOs;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication;

/// <summary>
/// Interface que define os contratos para os serviços de autenticação e gerenciamento de tokens.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica um usuário com base no e-mail e senha fornecidos.
    /// </summary>
    /// <param name="email">O e-mail do usuário.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Um objeto <see cref="TokenResponse"/> contendo o token de acesso gerado.</returns>
    /// <exception cref="TickestException">Lançado se o usuário não for encontrado, as credenciais forem inválidas ou o usuário estiver inativo/deletado.</exception>
    Task<TokenResponse> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o usuário atualmente autenticado com base no token JWT.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>O objeto <see cref="User"/> correspondente ao usuário autenticado.</returns>
    /// <exception cref="TickestException">Lançado se o usuário não for encontrado ou não estiver autenticado.</exception>
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Renova um token de acesso com base em um refresh token válido.
    /// </summary>
    /// <param name="refreshToken">O token de atualização (refresh token) fornecido pelo cliente.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Um objeto <see cref="Result{T}"/> contendo o novo token de acesso.</returns>
    /// <exception cref="TickestException">Lançado se o refresh token for inválido ou expirado.</exception>
    Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
