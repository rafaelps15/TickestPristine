namespace Tickest.Domain.Entities;

#region UserRole
/// <summary>
/// Representa o relacionamento entre um usuário e uma função (papel) no sistema.
/// Esta classe mapeia a atribuição de funções a usuários, estabelecendo um relacionamento muitos para muitos.
/// </summary>
public class UserRole : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public string Description { get; set; }
}
#endregion