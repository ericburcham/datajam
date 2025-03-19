namespace DataJam;

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

/// <summary>Exposes an interface for requests that return collections.</summary>
/// <typeparam name="T">The type of the expected result set.</typeparam>
[PublicAPI]
public interface IQuery<out T>
{
    /// <summary>Executes the request against the specified data source and returns the results.</summary>
    /// <param name="dataSource">The data source to execute the query against.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> containing the results of the query.</returns>
    public IEnumerable<T> Execute(IDataSource dataSource);
}

/// <summary>Exposes an interface for requests that return collections with a selectable projection.</summary>
/// <typeparam name="TSelection">The source <see cref="Type" /> of the query.</typeparam>
/// <typeparam name="TProjection">The projection <see cref="Type" /> of the query.</typeparam>
public interface IQuery<out TSelection, out TProjection> : IQuery<TProjection>;
