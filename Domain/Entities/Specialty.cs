namespace Tickest.Domain.Entities;

#region Specialty
/// <summary>
/// Representa uma especialidade dentro de uma área, indicando uma área de especialização específica.
/// Cada especialidade está associada a uma área maior (Area).
/// </summary>
public class Specialty : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid AreaId { get; set; }
    public Area Area { get; set; }

    // Relacionamento com os usuários que têm essa especialidade
    public ICollection<UserSpecialty> UserSpecialties { get; set; }

}
#endregion