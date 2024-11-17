namespace Tickest.Domain.Entities;

#region UserPermission
/// <summary>
/// Representa o relacionamento entre um usuário e uma permissão no sistema.
/// Esta classe mapeia a atribuição de permissões específicas a usuários (relacionamento muitos para muitos).
/// </summary>
public class UserPermission : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }
}
#endregion
