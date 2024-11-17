namespace Tickest.Domain.Entities;

#region Sector
/// <summary>
/// Representa um setor dentro da organização, que pode ser uma divisão geográfica ou funcional.
/// Cada setor pode ter múltiplos departamentos associados.
/// </summary>
public class Sector : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Department> Departments { get; set; }
}
#endregion
