
using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Permissions;

public class Role : EntityBase
{
    public string Description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}
