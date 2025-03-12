namespace DataJam;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using JetBrains.Annotations;

/// <summary>Provides a base class for queries.</summary>
/// <typeparam name="T">The type of the expected result set.</typeparam>
[PublicAPI]
public abstract class Query<T> : IQuery<T>
{
    /// <summary>Gets or sets the query to execute.</summary>
    protected Func<IDataSource, IQueryable<T>> Selector { get; set; } = null!;

    /// <inheritdoc cref="IQuery{T}" />
    public IEnumerable<T> Execute(IDataSource dataSource)
    {
        return Selector(dataSource);
    }

    /// <summary>Supports building simple fluent API by internally doing the work necessary to alter the current <see cref="Selector" />.</summary>
    /// <param name="predicate">The predicate to apply to the current data request.</param>
    /// <returns>The current <see cref="Query{T}" /> with the given <paramref name="predicate" /> applied.</returns>
    protected Query<T> AddPredicate(Expression<Func<T, bool>> predicate)
    {
        var currentSelector = Selector;
        Selector = dataSource => currentSelector(dataSource).Where(predicate);

        return this;
    }
}
