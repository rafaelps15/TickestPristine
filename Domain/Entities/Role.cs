using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Permissions;

public class Role : EntityBase
{
    public string Name { get; set; } // Exemplo: "Admin", "Collaborator", "Manager"
    public string Description { get; set; } // Descrição do papel (opcional)

    // Relacionamento muitos-para-muitos com Permissions(Um papel pode ter várias permissões)
    public ICollection<Permission> Permissions { get; set; } 

    // Relacionamento um-para-muitos com User (um papel pode ter vários usuários)
    public ICollection<User> Users { get; set; }  
}



