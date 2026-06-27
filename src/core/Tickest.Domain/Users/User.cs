using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.SharedKernel;

namespace Tickest.Domain.Entities.Users;

public class User : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string EmployeeCode { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public EntityId RoleId { get; private set; } = null!;
    public Role Role { get; private set; } = null!;
    public EntityId? SectorId { get; private set; }
    public Sector? Sector { get; private set; }
    public ICollection<UserSpecialty> UserSpecialties { get; private set; } = [];
    public ICollection<Permission> Permissions { get; private set; } = [];

    private User()
    {
    }

    public static User Create(
        string name,
        string employeeCode,
        string email,
        string passwordHash,
        EntityId roleId,
        EntityId? sectorId)
    {
        return new User
        {
            Name = name,
            EmployeeCode = employeeCode,
            Email = email,
            PasswordHash = passwordHash,
            RoleId = roleId,
            SectorId = sectorId
        };
    }

    public void AddSpecialty(EntityId specialtyId)
    {
        UserSpecialties.Add(UserSpecialty.Create(Id, specialtyId));
    }
}
