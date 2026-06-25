using Tickest.Application.DTOs;
using Tickest.SharedKernel;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication;

public interface IAuthService
{
    Task<TokenResponse> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);

    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task<User> ValidateUserCredentialsAsync(string email, string password, CancellationToken cancellationToken);

    Task RehashPasswordAsync(User user, string password, CancellationToken cancellationToken);
}
