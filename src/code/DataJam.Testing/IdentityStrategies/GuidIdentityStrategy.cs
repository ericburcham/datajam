namespace DataJam.Testing;

using System;
using System.Linq.Expressions;

internal class GuidIdentityStrategy<T> : IdentityStrategy<T, Guid>
    where T : class
{
    public GuidIdentityStrategy(Expression<Func<T, Guid>> propertyExpression)
        : base(propertyExpression)
    {
        Generator = GenerateGuid;
    }

    protected override bool DefaultValueIsUnset(Guid id)
    {
        return id == Guid.Empty;
    }

    private Guid GenerateGuid()
    {
        SetLastValue(Guid.NewGuid());

        return LastValue;
    }
}
