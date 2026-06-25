using Tickest.SharedKernel.Exceptions;

namespace Tickest.Domain.Entities.Base;

public abstract class EntityBase
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeactivatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void Activate()
    {
        if (IsDeleted)
        {
            throw new TickestException("A entidade deletada não pode ser ativada.");
        }

        IsActive = true;
        DeactivatedAt = null;
        MarkAsUpdated();
    }

    public void Deactivate()
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
        DeactivatedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void SoftDelete()
    {
        if (IsDeleted)
        {
            throw new TickestException("A entidade já foi deletada.");
        }

        IsDeleted = true;
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
