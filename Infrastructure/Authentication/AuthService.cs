using Microsoft.AspNetCore.Http;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Exceptions;

namespace Tickest.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new TickestException("Usuário não autenticado.");
        return Guid.Parse(userId);
    }

    public string GetCurrentUserEmail()
    {
        var email = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new TickestException("Usuário não autenticado.");
        return email;
    }
}
