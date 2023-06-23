namespace DataJam;

using System;
using System.Collections.Generic;

/// <summary>Exposes an interface for requests that return collections.</summary>
/// <typeparam name="TResult">The return <see cref="Type" /> of the query.</typeparam>
public interface IQuery<out TResult>
{
    /// <summary>Executes the request against the specified data source and returns the results.</summary>
    /// <param name="dataSource">The data source to execute the query against.</param>
    /// <returns>An <see cref="IEnumerable{TResult}" /> containing the results of the query.</returns>
    public IEnumerable<TResult> Execute(IDataSource dataSource);
}

/// <summary>Exposes an interface for requests that return collections with a selectable projection.</summary>
/// <typeparam name="TSelection">The source <see cref="Type" /> of the query.</typeparam>
/// <typeparam name="TProjection">The projection <see cref="Type" /> of the query.</typeparam>
public interface IQuery<out TSelection, out TProjection> : IQuery<TProjection>
{
}
