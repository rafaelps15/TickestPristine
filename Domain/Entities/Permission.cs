using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Permissions;

public class Permission : EntityBase
{
    public string Name { get; set; } // Exemplo: "ManageTickets", "ViewReports"
    public string Description { get; set; }

    // Relacionamento N:N com Roles através de RolePermission
    public ICollection<RolePermission> RolePermissions { get; set; }
}
