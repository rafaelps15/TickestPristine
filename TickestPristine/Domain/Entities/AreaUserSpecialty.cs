using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Specialties;

#region AreaUserSpecialty
public class AreaUserSpecialty
{
    public Guid AreaId { get; set; }
    public Area Area { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
}
#endregion
