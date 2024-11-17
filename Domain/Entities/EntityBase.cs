namespace Tickest.Domain.Entities;

#region Base Entity
/// <summary>
/// Classe base que contém o Id comum para todas as entidades.
/// </summary>
public abstract class EntityBase
{
    public Guid Id { get; set; }
}    
#endregion