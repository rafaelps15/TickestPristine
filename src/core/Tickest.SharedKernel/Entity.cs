namespace Tickest.SharedKernel;

public abstract class Entity<TEntityId>
    where TEntityId : ValueObject
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TEntityId Id { get; protected init; } = default!;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

public abstract class Entity : Entity<EntityId>
{
    protected Entity()
    {
        Id = EntityId.New();
    }
}
