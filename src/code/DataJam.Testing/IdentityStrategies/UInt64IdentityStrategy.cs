namespace DataJam.Testing;

using System;
using System.Linq.Expressions;

internal class UInt64IdentityStrategy<T> : IdentityStrategy<T, ulong>
    where T : class
{
    public UInt64IdentityStrategy(Expression<Func<T, ulong>> propertyExpression)
        : base(propertyExpression)
    {
        Generator = GenerateInt64;
    }

    protected override bool DefaultValueIsUnset(ulong id)
    {
        return id == 0;
    }

    private ulong GenerateInt64()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}
