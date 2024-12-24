using Tickest.Application.DTOs;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication;

/// <summary>
/// Interface responsável pela geração de tokens JWT.
/// </summary>
public interface ITokenProvider
{
    /// <summary>
    /// Gera um token JWT para um usuário autenticado.
    /// </summary>
    /// <param name="user">O usuário para o qual o token será gerado.</param>
    /// <returns>Um objeto <see cref="TokenResponse"/> contendo o token gerado e a data de expiração.</returns>
    //TokenResponse GenerateToken(User user);

    string Create(User user);
}
