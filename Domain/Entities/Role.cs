using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Permissions;

public class Role : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<User> Users { get; set; }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }
}



