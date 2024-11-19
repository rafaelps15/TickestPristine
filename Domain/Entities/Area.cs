namespace Tickest.Domain.Entities;

#region Area
/// <summary>
/// Área: Representa uma subdivisão de um departamento.
/// </summary>
public class Area : EntityBase
{
    public string Name { get; set; }  // Nome da área (ex: Suporte, Desenvolvimento, etc.)
    public string Description { get; set; }  // Descrição da área
    public Guid DepartmentId { get; set; }  // O Departamento ao qual essa Área pertence
    public Department Department { get; set; }  // Relacionamento com o Departamento

    // Relacionamento: Uma Área pode ter várias Especialidades.
    public ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
}
#endregion