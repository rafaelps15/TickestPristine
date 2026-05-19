using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Specialties;

#region Specialty
public class Specialty : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relacionamento N:N com áreas e usuários
    public ICollection<AreaUserSpecialty> AreaUserSpecialties { get; set; } = [];
    public ICollection<UserSpecialty> UserSpecialties { get; set; } = [];
}
#endregion
