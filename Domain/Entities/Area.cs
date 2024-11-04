namespace Tickest.Domain.Entities;

public class Area : EntityBase
{
    public string Description { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<SupportTicket> Tickets { get; set; }
}
