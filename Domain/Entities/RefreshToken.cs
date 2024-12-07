using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Auths;

#region RefreshToken
/// <summary>
/// Token de atualização usado para obter novos tokens de acesso.
/// </summary>
public class RefreshToken : EntityBase
{
    public Guid UserId { get; set; } 
    public string Token { get; set; } 
    public DateTimeOffset ExpiresAt { get; set; } // Data e hora de expiração do token
    public bool IsRevoked { get; set; } // Indica se o token foi revogado
    public bool IsUsed { get; set; } // Indica se o token foi usado

    /// <summary>
    /// Verifica se o token é válido.
    /// </summary>
    /// <returns>Retorna true se o token for válido, caso contrário, false.</returns>
    public bool IsValid() => !IsUsed && !IsRevoked && DateTimeOffset.UtcNow < ExpiresAt;
}
#endregion
