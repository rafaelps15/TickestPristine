using System.Threading;
using System.Threading.Tasks;
using Tickest.Application.DTOs;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication;

/// <summary>
/// Interface responsável pela autenticação de usuários, gerando tokens de acesso e realizando a renovação de tokens.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica um usuário com base no email e senha fornecidos.
    /// </summary>
    /// <param name="email">O email do usuário.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Um objeto contendo o token de acesso gerado.</returns>
    Task<TokenResponse> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera o usuário atualmente autenticado, com base nas informações do contexto HTTP.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto de usuário do tipo <see cref="User"/>.</returns>
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Renova o token de acesso utilizando um refresh token válido.
    /// </summary>
    /// <param name="refreshToken">O refresh token do usuário.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Um objeto contendo o novo token de acesso.</returns>
    Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Realiza a revalidação da senha e rehash, caso necessário.
    /// </summary>
    /// <param name="password">A senha do usuário.</param>
    /// <param name="passwordHash">O hash da senha armazenado no banco de dados.</param>
    /// <returns>O novo hash de senha, caso a revalidação seja necessária, ou null se não for necessário.</returns>
    string? RehashIfNeeded(string password, string passwordHash);
}
