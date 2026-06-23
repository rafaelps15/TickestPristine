using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tickest.Domain.Exceptions;


namespace Tickest.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    #region Obter ID do Usuário

    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new TickestException("ID do usuário não é válido.");
    }
        

    #endregion

    #region Obter Papel do Usuário

    public static string GetUserRole(this ClaimsPrincipal? principal) =>
        principal?.FindFirstValue(ClaimTypes.Role)
        ?? throw new TickestException("Papel do usuário não está disponível.");

    #endregion

    public static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
    {
        return principal?.FindFirst(claimType)?.Value;
    }
}
