using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

#region Sector
public class Sector : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relacionamento com departamento
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // Responsável pelo setor
    public Guid ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; } = null!;

    // Relação 1:N com áreas
    public ICollection<Area> Areas { get; set; } = [];
}
#endregion
