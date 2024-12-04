using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Specialties;

#region Specialty
/// <summary>
/// Specialty: Representa uma especialidade dentro de uma área, indicando uma área de especialização específica.
/// </summary>
public class Specialty : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Relacionamento N:N com áreas e usuários
    public ICollection<AreaUserSpecialty> AreaUserSpecialties { get; set; }
    public ICollection<UserSpecialty> UserSpecialties { get; set; }
}
#endregion