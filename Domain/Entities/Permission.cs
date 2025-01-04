using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Permissions;

public class Permission : EntityBase
{
    public string Name { get; set; } // Exemplo: "ManageTickets", "ViewReports"
    public string Description { get; set; }

    // Relacionamento com Role (muitos-para-muitos)
    public ICollection<Role> Roles { get; set; }

    // Relacionamento N:N com User (muitos-para-muitos)
    public ICollection<User> Users { get; set; }
}
