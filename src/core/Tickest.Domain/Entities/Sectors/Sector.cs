using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;

namespace Tickest.Domain.Entities.Sectors;

public class Sector : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public EntityId DepartmentId { get; private set; } = null!;
    public Department Department { get; private set; } = null!;
    public EntityId ResponsibleUserId { get; private set; } = null!;
    public User ResponsibleUser { get; private set; } = null!;
    public ICollection<User> Users { get; private set; } = [];

    private Sector()
    {
    }
}
