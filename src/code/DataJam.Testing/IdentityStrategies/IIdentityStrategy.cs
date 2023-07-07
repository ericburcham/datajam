namespace DataJam.Testing;

/// <summary>
///     Provides a contract for Types that provide an identity strategy for the TestDataContext.
/// </summary>
/// <typeparam name="T">
///     The element Type.
/// </typeparam>
public interface IIdentityStrategy<in T>
    where T : class
{
    void Assign(T entity);
}
