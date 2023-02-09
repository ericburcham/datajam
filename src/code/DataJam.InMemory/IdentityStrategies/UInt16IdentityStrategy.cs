using System.Linq.Expressions;

namespace DataJam.InMemory.IdentityStrategies;

public class UInt16IdentityStrategy<T> : IdentityStrategy<T, ushort>
    where T : class
{
    public UInt16IdentityStrategy(Expression<Func<T, ushort>> property)
        : base(property)
    {
        Generator = GenerateUInt16;
    }

    protected override bool IsDefaultUnsetValue(ushort id)
    {
        return id == default;
    }

    private ushort GenerateUInt16()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}