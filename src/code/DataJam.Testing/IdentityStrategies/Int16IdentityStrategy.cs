namespace DataJam.Testing;

using System;
using System.Linq.Expressions;

internal class Int16IdentityStrategy<T> : IdentityStrategy<T, short>
    where T : class
{
    public Int16IdentityStrategy(Expression<Func<T, short>> propertyExpression)
        : base(propertyExpression)
    {
        Generator = GenerateInt16;
    }

    protected override bool DefaultValueIsUnset(short id)
    {
        return id == default;
    }

    private short GenerateInt16()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}
