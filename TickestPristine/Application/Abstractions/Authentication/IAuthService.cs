using Tickest.Application.DTOs;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication;

/// <summary>
/// Interface que define os métodos de autenticação e gerenciamento de usuários.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica o usuário com base no e-mail e senha fornecidos.
    /// </summary>
    /// <param name="email">O e-mail do usuário.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <param name="cancellationToken">O token de cancelamento da operação assíncrona.</param>
    /// <returns>Um objeto <see cref="TokenResponse"/> contendo o token gerado.</returns>
    Task<TokenResponse> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o usuário atual baseado no contexto da requisição.
    /// </summary>
    /// <param name="cancellationToken">O token de cancelamento da operação assíncrona.</param>
    /// <returns>O usuário atual.</returns>
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Renova o token de acesso com base no refresh token fornecido.
    /// </summary>
    /// <param name="refreshToken">O refresh token do usuário.</param>
    /// <param name="cancellationToken">O token de cancelamento da operação assíncrona.</param>
    /// <returns>O novo token de acesso.</returns>
    Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Valida as credenciais do usuário com base no e-mail e senha fornecidos.
    /// </summary>
    /// <param name="email">O e-mail do usuário.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <param name="cancellationToken">O token de cancelamento da operação assíncrona.</param>
    /// <returns>O usuário autenticado.</returns>
    Task<User> ValidateUserCredentialsAsync(string email, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Recalcula e atualiza o hash da senha do usuário se necessário.
    /// </summary>
    /// <param name="user">O usuário cujas credenciais precisam ser atualizadas.</param>
    /// <param name="password">A senha do usuário.</param>
    /// <param name="cancellationToken">O token de cancelamento da operação assíncrona.</param>
    /// <returns>A tarefa assíncrona que representa a operação.</returns>
    Task RehashPasswordAsync(User user, string password, CancellationToken cancellationToken);
}
