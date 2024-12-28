using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Departments;

#region Department
/// <summary>
/// Departamento: Representa um departamento dentro da organização (ex: TI, Marketing).
/// </summary>
public class Department : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Relacionamento com setores
    public ICollection<Sector> Sectors { get; set; }

    // Responsável pelo departamento
    public Guid? ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; }
}
#endregion