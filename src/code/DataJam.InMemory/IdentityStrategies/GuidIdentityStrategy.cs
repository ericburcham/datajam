using System.Linq.Expressions;

namespace DataJam.InMemory.IdentityStrategies;

public class GuidIdentityStrategy<T> : IdentityStrategy<T, Guid>
    where T : class
{
    public GuidIdentityStrategy(Expression<Func<T, Guid>> property) : base(property)
    {
        Generator = GenerateGuid;
    }

    private Guid GenerateGuid()
    {
        SetLastValue(Guid.NewGuid());

        return LastValue;
    }

    protected override bool IsDefaultUnsetValue(Guid id)
    {
        return id == default;
    }
}