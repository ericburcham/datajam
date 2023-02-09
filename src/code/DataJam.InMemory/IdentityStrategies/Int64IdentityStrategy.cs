using System.Linq.Expressions;

namespace DataJam.InMemory.IdentityStrategies;

public class Int64IdentityStrategy<T> : IdentityStrategy<T, long>
    where T : class
{
    public Int64IdentityStrategy(Expression<Func<T, long>> property)
        : base(property)
    {
        Generator = GenerateInt64;
    }

    protected override bool IsDefaultUnsetValue(long id)
    {
        return id == default;
    }

    private long GenerateInt64()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}