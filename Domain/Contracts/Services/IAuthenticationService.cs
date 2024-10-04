using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Entities;

namespace Tickest.Domain.Contracts.Services;

public interface IAuthenticationService
{
    Task<TokenResponse> AuthenticateAsync(Usuario usuario);
    Task<string> RenewTokenAsync(string userId);
}
