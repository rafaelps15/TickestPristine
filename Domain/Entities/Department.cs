namespace Tickest.Domain.Entities;

#region Department
/// <summary>
/// Departamento: Representa um departamento dentro da organização, como TI, Marketing, etc.
/// </summary>
public class Department : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Area> Areas { get; set; }
    public ICollection<Sector> Sectors { get; set; }
}
#endregion