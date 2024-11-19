namespace Tickest.Domain.Entities;

#region Sector
/// <summary>
/// Representa um setor dentro da organização, que pode ser uma divisão geográfica ou funcional.
/// Cada setor pode ter múltiplos departamentos associados.
/// </summary>
public class Sector : EntityBase
{
    public string Name { get; set; }  // Nome do setor (ex: Comercial, Produção, etc.)
    public string Description { get; set; }  // Descrição do setor

    // Coleção de Departamentos no Setor -- Relacionamento um-para-muitos com Department
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}

#endregion
