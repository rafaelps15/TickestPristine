using System;
using Tickest.Domain.Entities.Base;
using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Entities;

public abstract class Entity<TId> : EntityBase<TId>
{
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; private set; } = false;   
    public DateTime CreatedAt { get; private set; }       
    public DateTime? DeactivatedAt { get; private set; }   
    public DateTime? UpdatedAt { get; private set; }      

    protected Entity()
    {
        CreatedAt = DateTime.UtcNow;  
    }

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

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;  
    }
}
