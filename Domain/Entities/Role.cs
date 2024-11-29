namespace Tickest.Domain.Entities;

#region Role
/// <summary>
/// Representa um papel (Role) no sistema.
/// </summary>
public class Role : EntityBase
{
    public string Name { get; set; }
    public string? Description { get; set; }

    // Permissões associadas a esse papel via RolePermission
    public ICollection<UserRole> UserRoles { get; set; } 
    public ICollection<Permission> Permissions { get; private set; }

}
#endregion


/*
 * 
 * ROLE
 * 
 * ADMIN (TICKET -> ATUALIZAR), (USER -> CRIAR, ATUALIZAR, DELETAR), (DEPARTAMENTO -> CRIAR, ATUALIZAR, DELETAR)
 * PUBLIC (TICKET -> VISUALIZAR)
 * ANALISTA ()
 * 
 * PERMISSOES (CRIAR, ATUALIZAR, DELETAR)
 */