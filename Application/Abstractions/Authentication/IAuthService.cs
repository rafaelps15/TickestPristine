using Tickest.Domain.Common;
using Tickest.Domain.Entities;

namespace Tickest.Application.Abstractions.Authentication;

public interface IAuthService
{
    /// <summary>
    /// Authenticates the user and returns a JWT token and refresh token.
    /// </summary>
    /// <param name="user">The user to authenticate.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the authentication result with tokens.</returns>
    Task<TokenResponse> AuthenticateAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the currently authenticated user from the HTTP context.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The authenticated user, or null if no user is authenticated.</returns>
    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Renews the JWT token using the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token used to obtain a new JWT token.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result with the new JWT token.</returns>
    Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the user associated with a given refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to look for.</param>
    /// <returns>The user associated with the refresh token.</returns>
    Task<User> GetUserByRefreshTokenAsync(string refreshToken);
  
}
