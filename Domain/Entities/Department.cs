using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Sectors;
/// <summary>
/// Representa um departamento dentro de um setor. Cada departamento tem um foco específico 
/// dentro da organização, como TI, Marketing, etc.
/// </summary>
public class Department : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid SectorId { get; set; } // Relacionamento 1:N com o setor
    public Sector Sector { get; set; } // Setor ao qual o departamento pertence
    public Guid? DepartmentManagerId { get; set; }// Gestor do departamento
    public User DepartmentManager { get; set; } // Relacionamento 1:1 com o gestor do departamento
    public List<Area> Areas { get; set; } // Relacionamento 1:N com as áreas

}