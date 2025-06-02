using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tickest.Domain.Exceptions;


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
            : throw new TickestException("ID do usuário não está disponível.");
    //GetUserId: Pega o sub (subject) do JWT, que guarda o ID do usuário, e converte para Guid.
    #endregion

    #region Obter Papel do Usuário

    /// <summary>
    /// Obtém o papel do usuário a partir do token JWT.
    /// </summary>
    public static string GetUserRole(this ClaimsPrincipal? principal) =>
        principal?.FindFirstValue(ClaimTypes.Role)
        ?? throw new TickestException("Papel do usuário não está disponível.");
    //Retorna a primeira claim do tipo Role (ClaimTypes.Role) — ou lança exceção se não encontrar.
    #endregion


}
