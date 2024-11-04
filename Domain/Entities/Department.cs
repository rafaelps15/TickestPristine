namespace Tickest.Domain.Entities;

public class Department : EntityBase
{
    public string Name { get; set; }

    public int ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; }

    public ICollection<Area> Areas { get; set; }
}
