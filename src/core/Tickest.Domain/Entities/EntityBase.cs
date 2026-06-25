using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Domain.Entities.Base;

public abstract class EntityBase : Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected EntityBase()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void Activate(DateTime utcNow)
    {
        if (IsDeleted)
        {
            throw new TickestException("A entidade deletada não pode ser ativada.");
        }

        IsActive = true;
        DeactivatedAt = null;
        MarkAsUpdated(utcNow);
    }

    public void Deactivate(DateTime utcNow)
    {
        if (IsDeleted)
        {
            throw new TickestException("A entidade deletada não pode ser desativada.");
        }

        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        DeactivatedAt = utcNow;
        MarkAsUpdated(utcNow);
    }

    public void SoftDelete(DateTime utcNow)
    {
        if (IsDeleted)
        {
            throw new TickestException("A entidade já foi deletada.");
        }

        IsDeleted = true;
        IsActive = false;
        DeletedAt = utcNow;
        MarkAsUpdated(utcNow);
    }

    public void MarkAsUpdated(DateTime utcNow)
    {
        UpdatedAt = utcNow;
    }
}
