using System.Runtime.InteropServices;
using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Permissions;

public class Role : EntityBase<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }

    public Role() { }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }
}



