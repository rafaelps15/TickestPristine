using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

#region Area
public class Area : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relacionamento com setor
    public Guid SectorId { get; set; }
    public Sector Sector { get; set; } = null!;

    // Responsável pela área
    public Guid ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; } = null!;


    // Relação N:N com usuários e especialidades
    public ICollection<AreaUserSpecialty> AreaUserSpecialties { get; set; } = [];

    // Relacionamento com Specialty
    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
}
#endregion
