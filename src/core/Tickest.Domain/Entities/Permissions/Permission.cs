using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Permissions;

public class Permission : AuditableEntity
{
    public string Description { get; set; } = string.Empty;
}
