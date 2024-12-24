using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Specialties;

#region AreaUserSpecialty
/// <summary>
/// AreaUserSpecialty: Representa a relação entre um usuário e uma especialidade dentro de uma área.
/// </summary>
public class AreaUserSpecialty
{
    public Guid AreaId { get; set; }
    public Area Area { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; }
}
#endregion