using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Exceptions;

namespace Tickest.Infrastructure.Authentication;

/// <summary>
/// Serviço responsável por acessar informações do usuário autenticado
/// através das claims presentes no HttpContext.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal GetPrincipal()
    {
        return _httpContextAccessor.HttpContext?.User
            ?? throw new TickestException("Usuário não autenticado.");
    }

    /// <summary>
    /// Retorna o Id do usuário autenticado.
    /// </summary>
    public Guid GetCurrentUserId()
    {
        return GetPrincipal().GetUserId();
    }

    /// <summary>
    /// Retorna todas as roles do usuário autenticado.
    /// </summary>
    public IEnumerable<string> GetUserRoles()
    {
        return GetPrincipal().GetUserRoles();
    }

    /// <summary>
    /// Verifica se o usuário autenticado possui uma role específica.
    /// </summary>
    public bool IsInRole(string role)
    {
        return GetPrincipal().IsInRole(role);
    }
}