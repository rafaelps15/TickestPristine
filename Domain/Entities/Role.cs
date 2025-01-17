using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Permissions;

public class Role : EntityBase
{
    public string Name { get; set; } // Exemplo: "Admin", "Collaborator", "Manager"
    public string Description { get; set; } // Descrição do papel (opcional)

    // Relacionamento N:N com Permissions através de RolePermission
    public ICollection<RolePermission> RolePermissions { get; set; }

    // Relacionamento 1:N com User (um papel pode ser atribuído a vários usuários)
    public ICollection<User> Users { get; set; }
}



