using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    #region Obter ID do Usuário
    /// <summary>
    /// Obtém o ID do usuário a partir do token JWT.
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal? principal) =>
        Guid.TryParse(principal?.FindFirstValue(JwtRegisteredClaimNames.Sub), out var userId)
            ? userId
            : throw new ApplicationException("ID do usuário não está disponível.");
    #endregion

    #region Obter Papel do Usuário
    /// <summary>
    /// Obtém o papel do usuário a partir do token JWT.
    /// </summary>
    public static string GetUserRole(this ClaimsPrincipal? principal) =>
        principal?.FindFirstValue(ClaimTypes.Role)
        ?? throw new ApplicationException("Papel do usuário não está disponível.");
    #endregion
}
