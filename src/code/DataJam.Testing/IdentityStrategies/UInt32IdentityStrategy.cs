namespace DataJam.Testing;

using System.Linq.Expressions;

internal class UInt32IdentityStrategy<T> : IdentityStrategy<T, uint>
    where T : class
{
    public UInt32IdentityStrategy(Expression<Func<T, uint>> property)
        : base(property)
    {
        Generator = GenerateUInt32;
    }

    protected override bool IsDefaultUnsetValue(uint id)
    {
        return id == default;
    }

    private uint GenerateUInt32()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}
