namespace DataJam.Testing;

using System;
using System.Linq.Expressions;

internal class UInt32IdentityStrategy<T> : IdentityStrategy<T, uint>
    where T : class
{
    public UInt32IdentityStrategy(Expression<Func<T, uint>> propertyExpression)
        : base(propertyExpression)
    {
        Generator = GenerateUInt32;
    }

    protected override bool DefaultValueIsUnset(uint id)
    {
        return id == 0;
    }

    private uint GenerateUInt32()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}
