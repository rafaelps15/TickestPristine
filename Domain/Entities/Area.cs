namespace Tickest.Domain.Entities;

#region Area
/// <summary>
/// Área: Representa uma subdivisão de um departamento.
/// </summary>
public class Area : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<Specialty> Specialties { get; set; }
}
#endregion