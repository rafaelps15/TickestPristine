using Microsoft.AspNetCore.Http;
using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Services.Auth;

internal class AuthService : IAuthService
{
    public readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor=httpContextAccessor;
    }

    public Task<TokenModel> AuthenticateAsync(Usuario usuario)
    {
        throw new NotImplementedException();
    }

    public Task<Usuario> GetCurrentUserAsync()
    {
        throw new NotImplementedException();
    }
}
