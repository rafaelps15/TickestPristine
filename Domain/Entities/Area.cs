using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Sectors;

/// <summary>
/// Área: Representa uma subdivisão de um departamento.
/// </summary>
/// <summary>
/// Representa uma área dentro de um departamento, com foco em uma função ou atividade específica.
/// </summary>
public class Area : Entity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid DepartmentId { get; set; }  // Chave estrangeira para o Departamento ao qual a área pertence
    public Department Department { get; set; }  // Relacionamento N:1 com o Departamento (uma área pertence a um departamento)

    public Guid? AreaManagerId { get; set; }
    public User AreaManager { get; set; }

    public List<User> Users { get; set; }  // Relacionamento N:N com os Usuários (uma área pode ter vários usuários)

    public Guid SpecialtyId { get; set; }
    public List<Specialty> Specialties { get; set; }
}
