using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;

namespace Tickest.Domain.Entities.Specialties;

public class UserSpecialty
{
    public EntityId UserId { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public EntityId SpecialtyId { get; private set; } = null!;
    public Specialty Specialty { get; private set; } = null!;

    private UserSpecialty()
    {
    }

    public static UserSpecialty Create(EntityId userId, EntityId specialtyId)
    {
        return new UserSpecialty
        {
            UserId = userId,
            SpecialtyId = specialtyId
        };
    }
}
