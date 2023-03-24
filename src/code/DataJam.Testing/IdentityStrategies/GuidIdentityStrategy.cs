namespace DataJam.Testing;

using System.Linq.Expressions;

internal class GuidIdentityStrategy<T> : IdentityStrategy<T, Guid>
    where T : class
{
    public GuidIdentityStrategy(Expression<Func<T, Guid>> property)
        : base(property)
    {
        Generator = GenerateGuid;
    }

    protected override bool IsDefaultUnsetValue(Guid id)
    {
        return id == default;
    }

    private Guid GenerateGuid()
    {
        SetLastValue(Guid.NewGuid());

        return LastValue;
    }
}
