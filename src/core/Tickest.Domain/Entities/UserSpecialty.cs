using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Specialties;

#region UserSpecialty
public class UserSpecialty 
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
}
#endregion
