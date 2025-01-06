using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Sectors;

/// <summary>
/// Representa um setor dentro da organização, o nível mais alto da estrutura organizacional,
/// com várias divisões internas (departamentos). Cada setor tem um gestor (manager) responsável.
/// </summary>
public class Sector : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid? SectorManagerId { get; set; }    // Chave estrangeira para o Gestor (Manager)
    public User SectorManager { get; set; }     // Relacionamento 1:1 com o Usuário (Gestor do setor)

    public List<Department> Departments { get; set; } // Relacionamento 1:N com os departamentos (um setor pode ter vários departamentos)
}
