namespace DataJam.Testing;

using System;
using System.Linq.Expressions;

internal class Int64IdentityStrategy<T> : IdentityStrategy<T, long>
    where T : class
{
    public Int64IdentityStrategy(Expression<Func<T, long>> propertyExpression)
        : base(propertyExpression)
    {
        Generator = GenerateInt64;
    }

    protected override bool DefaultValueIsUnset(long id)
    {
        return id == default;
    }

    private long GenerateInt64()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}
