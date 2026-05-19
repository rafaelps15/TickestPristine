using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Entities.Base;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeactivatedAt { get; private set; }
    public DateTime? UpdateAt { get; set; }

    public void SoftDelete()
    {
        if (IsDeleted)
        {
            throw new TickestException("A entidade já foi deletada.");
        }

        IsDeleted = true;
        IsActive = false;
        DeactivatedAt = DateTime.UtcNow;
    }
}
