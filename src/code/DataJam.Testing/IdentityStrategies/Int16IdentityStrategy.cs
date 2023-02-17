using System.Linq.Expressions;

namespace DataJam.Testing;

internal class Int16IdentityStrategy<T> : IdentityStrategy<T, short>
    where T : class
{
    public Int16IdentityStrategy(Expression<Func<T, short>> property)
        : base(property)
    {
        Generator = GenerateInt16;
    }

    protected override bool IsDefaultUnsetValue(short id)
    {
        return id == default;
    }

    private short GenerateInt16()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}