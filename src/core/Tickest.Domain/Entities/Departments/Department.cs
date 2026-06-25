using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

public class Department : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Sector> Sectors { get; set; } = [];
    public Guid? ResponsibleUserId { get; set; }
    public User? ResponsibleUser { get; set; }
}
