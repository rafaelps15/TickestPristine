using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Interfaces;

public interface IAuthenticationService
{
    Task<TokenModel> AuthenticateAsync(Usuario usuario);
    Task<string> RenewTokenAsync(string userId);
}
