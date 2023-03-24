namespace DataJam.Testing;

using System.Linq.Expressions;

internal class Int32IdentityStrategy<T> : IdentityStrategy<T, int>
    where T : class
{
    public Int32IdentityStrategy(Expression<Func<T, int>> property)
        : base(property)
    {
        Generator = GenerateInt32;
    }

    protected override bool IsDefaultUnsetValue(int id)
    {
        return id == default;
    }

    private int GenerateInt32()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}
