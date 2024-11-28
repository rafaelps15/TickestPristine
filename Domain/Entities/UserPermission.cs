using Tickest.Domain.Entities;

public class UserPermission : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }
}
