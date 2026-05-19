using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

#region Department
public class Department : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relacionamento com setores
    public ICollection<Sector> Sectors { get; set; } = [];

    // Responsável pelo departamento
    public Guid? ResponsibleUserId { get; set; }
    public User? ResponsibleUser { get; set; }
}
#endregion
