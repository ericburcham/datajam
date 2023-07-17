namespace DataJam;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

    /// <summary>Supports building simple fluent API by internally doing the work necessary to alter the current <see cref="Selector" />.</summary>
    /// <param name="predicate">The predicate to apply to the current data request.</param>
    /// <returns>The current <see cref="Query{TResult}" /> with the given <paramref name="predicate" /> applied.</returns>
    protected Query<TResult> AddPredicate(Expression<Func<TResult, bool>> predicate)
    {
        var currentSelector = Selector;
        Selector = dataSource => currentSelector(dataSource).Where(predicate);

        return this;
    }
}
