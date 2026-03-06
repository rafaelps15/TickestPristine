using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tickest.Domain.Exceptions;


namespace Tickest.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    #region Obter ID do Usuário

    /// <summary>
    /// Obtém o ID do usuário a partir do token JWT.
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal? principal) =>
        Guid.TryParse(principal?.FindFirstValue(JwtRegisteredClaimNames.Sub), out var userId)
            ? userId
            : throw new TickestException("ID do usuário não está disponível.");
    //GetUserId: Pega o sub (subject) do JWT, que guarda o ID do usuário, e converte para Guid.
    #endregion

    #region Obter Papel do Usuário

    /// <summary>
    /// Obtém todas as roles do usuário a partir do token JWT.
    /// </summary>
   public static IEnumerable<string> GetUserRoles(this ClaimsPrincipal? principal) =>
        principal?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            ?? Enumerable.Empty<string>();
    #endregion


}
