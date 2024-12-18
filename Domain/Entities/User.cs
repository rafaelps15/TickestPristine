using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Domain.Entities.Users;

#region User
/// <summary>
/// User: Representa um usuário no sistema, com suas informações e permissões associadas.
/// </summary>
public class User : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }

    // Relação N:N com especialidades
    public ICollection<UserSpecialty> UserSpecialties { get; set; }

    // Relação N:N com áreas e especialidades
    public ICollection<AreaUserSpecialty> AreaUserSpecialties { get; set; }

    // Permissões associadas ao usuário
    public ICollection<Permission> Permissions { get; set; }

    // Relação N:N com papéis
    public ICollection<UserRole> UserRoles { get; set; }
    public string Role { get; set; }
}
#endregion