namespace Tickest.SharedKernel;

public sealed class EntityId : ValueObject
{
    public EntityId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static EntityId New() => new(Guid.NewGuid());

    public static implicit operator Guid(EntityId id) => id.Value;

    public static implicit operator EntityId(Guid value) => new(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
