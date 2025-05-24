using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Permissions;

public class Role : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }
}



