using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para o repositório de RefreshTokens.
/// Fornece métodos para acessar e gerenciar os tokens de refresh, além de herdar operações genéricas.
/// </summary>
public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    /// <summary>
    /// Obtém um token de refresh ativo com base no token fornecido.
    /// </summary>
    /// <param name="token">O token de refresh a ser procurado.</param>
    /// <returns>O RefreshToken correspondente, se encontrado.</returns>
    Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém o usuário associado ao refresh token fornecido.
    /// Se o token não for encontrado ou o usuário não for encontrado, uma exceção é lançada.
    /// </summary>
    /// <param name="refreshToken">O refresh token a ser procurado.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação assíncrona.</param>
    /// <returns>O usuário associado ao refresh token.</returns>
    /// <exception cref="TickestException">Lançada se o refresh token ou o usuário não forem encontrados.</exception>
    Task<User> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
