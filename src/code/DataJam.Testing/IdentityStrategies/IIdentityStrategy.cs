namespace DataJam.Testing;

using JetBrains.Annotations;

/// <summary>Provides a contract for Types that provide an identity strategy for the TestDataContext.</summary>
/// <typeparam name="T">The element Type.</typeparam>
[PublicAPI]
public interface IIdentityStrategy<in T>
    where T : class
{
    void Assign(T entity);
}
