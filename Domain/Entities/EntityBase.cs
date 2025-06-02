using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Entities.Base;

public abstract class EntityBase<TId>
{
    public TId Id { get; set; }  
}
