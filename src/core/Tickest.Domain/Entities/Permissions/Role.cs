using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Permissions;

public class Role : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public ICollection<User> Users { get; private set; } = [];

    private Role()
    {
    }
}
