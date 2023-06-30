namespace DataJam.Testing;

using System;
using System.Linq.Expressions;

internal class UInt16IdentityStrategy<T> : IdentityStrategy<T, ushort>
    where T : class
{
    public UInt16IdentityStrategy(Expression<Func<T, ushort>> propertyExpression)
        : base(propertyExpression)
    {
        Generator = GenerateUInt16;
    }

    protected override bool DefaultValueIsUnset(ushort id)
    {
        return id == default;
    }

    private ushort GenerateUInt16()
    {
        SetLastValue(++LastValue);

        return LastValue;
    }
}
