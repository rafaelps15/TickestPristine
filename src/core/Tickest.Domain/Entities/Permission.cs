using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Permissions;

#region Permission
public class Permission : EntityBase
{
    public string Description { get; set; } = string.Empty;
}
#endregion
