namespace Tickest.Domain.Entities;

public class Role : EntityBase
{
    public string Name { get; set; }

    public ICollection<UserRole> UserRules { get; set; }
}
