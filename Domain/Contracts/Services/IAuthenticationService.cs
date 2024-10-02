using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;

namespace Tickest.Domain.Contracts.Services;

public interface IAuthenticationService
{
    Task<TokenModel> AuthenticateAsync(Usuario usuario);
    Task<string> RenewTokenAsync(string userId);
}
