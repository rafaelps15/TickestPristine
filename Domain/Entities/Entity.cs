using Tickest.Domain.Entities.Base;
using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Entities;

public abstract class Entity : EntityBase
{
    public bool IsActive { get; set; }
    public bool IsDeleted { get; private set; }   // Indica se a entidade foi deletada
    public DateTime CreatedAt { get; set; }  // Data de criação da entidade
    public DateTime? DeactivatedAt { get; set; }  // Data de desativação da entidade
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
