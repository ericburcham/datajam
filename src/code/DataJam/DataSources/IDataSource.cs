namespace DataJam;

using System.Linq;

using JetBrains.Annotations;

/// <summary>Exposes an interface for querying data sources.</summary>
[PublicAPI]
public interface IDataSource
{
    /// <summary>Exposes a query for <typeparamref name="T" /> which can be used to build further query expressions and execute the query.</summary>
    /// <typeparam name="T">The query's element type.</typeparam>
    /// <returns>A query for <typeparamref name="T" />.</returns>
    IQueryable<T> CreateQuery<T>()
        where T : class;
}
