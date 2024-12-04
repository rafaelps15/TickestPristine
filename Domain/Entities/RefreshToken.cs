using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Auths;

#region RefreshToken
/// <summary>
/// RefreshToken: Representa um token de atualização usado para obter novos tokens de acesso.
/// </summary>
public class RefreshToken : EntityBase
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}
#endregion