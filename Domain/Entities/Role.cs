namespace Tickest.Domain.Entities;

#region Role
/// <summary>
/// Representa um papel (Role) no sistema.
/// </summary>
public class Role : EntityBase
{
    public string Name { get; set; } // Nome do papel, exemplo: "Admin", "GestorTickets"
    public string Description { get; set; } // Descrição do papel, como "Administrador geral"

    // Permissões associadas a esse papel via RolePermission
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
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