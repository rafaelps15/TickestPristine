using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;

namespace Tickest.Domain.Entities.Departments;

public class Department : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public ICollection<Sector> Sectors { get; private set; } = [];
    public EntityId? ResponsibleUserId { get; private set; }
    public User? ResponsibleUser { get; private set; }

    private Department()
    {
    }
}
