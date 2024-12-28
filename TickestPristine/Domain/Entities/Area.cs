using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

#region Area
/// <summary>
/// Área: Representa uma subdivisão de um departamento.
/// </summary>
public class Area : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Relacionamento com setor
    public Guid SectorId { get; set; }
    public Sector Sector { get; set; }

    // Responsável pela área
    public Guid ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; }


    // Relação N:N com usuários e especialidades
    public ICollection<AreaUserSpecialty> AreaUserSpecialties { get; set; }

    // Relacionamento com Specialty
    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; }
}
#endregion