using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Services.Auth;

public interface IAuthService
{
    Task<TokenModel> AuthenticateAsync(Usuario usuario);

    Task<Usuario> GetCurrentUserAsync();
}
