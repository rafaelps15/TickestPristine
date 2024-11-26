using Tickest.Domain.Entities;

public class UserPermission
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }

    // Propriedades adicionais, como data de criação
    public DateTime CreatedAt { get; set; }
}
