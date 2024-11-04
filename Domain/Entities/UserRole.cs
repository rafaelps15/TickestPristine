namespace Tickest.Domain.Entities;

public class UserRole : EntityBase
{
    public int UserId { get; private set; }
    public User User { get; private set; }

    public int RuleId { get; private set; }
    public Role Rule { get; private set; }

    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RuleId = roleId;
    }
}
