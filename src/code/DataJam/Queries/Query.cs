namespace DataJam;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>Provides a base class for queries.</summary>
/// <typeparam name="TResult">The type of the expected result set.</typeparam>
public abstract class Query<TResult> : IQuery<TResult>
{
    /// <summary>Gets or sets the query to execute.</summary>
    protected Func<IDataSource, IQueryable<TResult>> Selector { get; set; } = null!;

    /// <inheritdoc cref="IQuery{TResult}" />
    public IEnumerable<TResult> Execute(IDataSource dataSource)
    {
        return Selector(dataSource);
    }
}
