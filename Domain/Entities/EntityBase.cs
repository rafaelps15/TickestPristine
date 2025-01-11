using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Entities.Base;

public abstract class EntityBase
{
    public Guid Id { get; set; }  
    public bool IsActive { get; set; }  
    public bool IsDeleted { get; private set; }   // Indica se a entidade foi deletada
    public DateTime CreatedAt { get; set; }  // Data de criação da entidade
    public DateTime? DeactivatedAt { get;  set; }  // Data de desativação da entidade
    public DateTime? UpdateAt { get; set; }  // Data de atualização (opcional)

    public void SoftDelete()
    {
        if (IsDeleted)
        {
            throw new TickestException("A entidade já foi deletada.");
        }

        IsDeleted = true;
        IsActive = false;
        DeactivatedAt = DateTime.Now;
    }
        
}
