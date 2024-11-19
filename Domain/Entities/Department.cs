namespace Tickest.Domain.Entities;

#region Department
/// <summary>
/// Departamento: Representa um departamento dentro da organização, como TI, Marketing, etc.
/// </summary>
public class Department : EntityBase
{
    public string Name { get; set; }  // Nome do departamento (ex: TI, RH, Marketing)
    public string Description { get; set; }  // Descrição do departamento

    // Chave estrangeira para Sector
    public Guid SectorId { get; set; }
    public Sector Sector { get; set; }

    // Coleção de Áreas dentro do Departamento
    public ICollection<Area> Areas { get; set; } = new List<Area>();
    
}
#endregion