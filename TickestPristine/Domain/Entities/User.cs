using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Domain.Entities.Users;

#region User
public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;

    // Relação N:N com especialidades
    public ICollection<UserSpecialty> UserSpecialties { get; set; } = [];

    // Relação N:N com áreas e especialidades
    public ICollection<AreaUserSpecialty> AreaUserSpecialties { get; set; } = [];

    // Permissões associadas ao usuário
    public ICollection<Permission> Permissions { get; set; } = [];

    // Relação N:N com papéis
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public string Role { get; set; } = string.Empty;
}
#endregion
