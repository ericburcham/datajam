using System.Linq.Expressions;

namespace DataJam.Testing;

internal class UInt64IdentityStrategy<T> : IdentityStrategy<T, ulong>
    where T : class
{
    public UInt64IdentityStrategy(Expression<Func<T, ulong>> property)
        : base(property)
    {
        Generator = GenerateInt64;
    }

    protected override bool IsDefaultUnsetValue(ulong id)
    {
        return id == default;
    }

    private ulong GenerateInt64()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}