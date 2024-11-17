using Tickest.Domain.Common;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Entities;

namespace Tickest.Application.Abstractions.Authentication;

public interface IAuthService
{
    Task<TokenResponse> AuthenticateAsync(User user); 
    Task<Result<string>> RenewTokenAsync(string refreshToken);
    Task<User> GetCurrentUserAsync();
}
