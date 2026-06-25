using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

public class Sector : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
    public Guid ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; } = null!;
    public ICollection<User> Users { get; set; } = [];
}
