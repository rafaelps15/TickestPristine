namespace Tickest.Domain.Entities;

#region RolePermission (Relacionamento Papel-Permissão)
/// <summary>
/// Esta classe mapeia as permissões atribuídas a um papel específico.
/// </summary>
public class RolePermission : EntityBase
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public Guid PermissionId { get; set; }
    //public Permission Permission { get; set; }
}
#endregion