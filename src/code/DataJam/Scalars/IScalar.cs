namespace DataJam;

/// <summary>Exposes an interface for scalar queries.</summary>
/// <typeparam name="TResult">The type of the expected result.</typeparam>
public interface IScalar<out TResult>
{
    /// <summary>Executes the request against the specified data source and returns the scalar value.</summary>
    /// <param name="dataSource">The data source to execute the query against.</param>
    /// <returns>The scalar value retrieved from the data source.</returns>
    TResult Execute(IDataSource dataSource);
}
