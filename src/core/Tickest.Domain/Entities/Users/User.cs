using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Domain.Entities.Users;

public class User : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Guid? SectorId { get; set; }
    public Sector? Sector { get; set; }
    public ICollection<UserSpecialty> UserSpecialties { get; set; } = [];
    public ICollection<Permission> Permissions { get; set; } = [];
}
