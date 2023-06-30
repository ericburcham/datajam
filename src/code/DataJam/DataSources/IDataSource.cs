namespace DataJam;

using System.Linq;

/// <summary>Exposes an interface for querying data sources.</summary>
public interface IDataSource
{
    /// <summary>Exposes a query for <typeparamref name="TResult" /> which can be used to build further query expressions and execute the query.</summary>
    /// <typeparam name="TResult">The query's element type.</typeparam>
    /// <returns>A query for <typeparamref name="TResult" />.</returns>
    IQueryable<TResult> AsQueryable<TResult>()
        where TResult : class;
}
