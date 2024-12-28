using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

#region Sector
/// <summary>
/// Sector: Representa um setor dentro de um departamento, como "Desenvolvimento", "Marketing", etc.
/// </summary>
public class Sector : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Relacionamento com departamento
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; }

    // Responsável pelo setor
    public Guid ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; }

    // Relação 1:N com áreas
    public ICollection<Area> Areas { get; set; }
}
#endregion