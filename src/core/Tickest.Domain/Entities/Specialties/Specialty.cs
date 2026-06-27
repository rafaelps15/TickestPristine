using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Specialties;

public class Specialty : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public ICollection<UserSpecialty> UserSpecialties { get; private set; } = [];

    private Specialty()
    {
    }
}
