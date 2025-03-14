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

    /// <inheritdoc cref="IQuery{T}.Execute" />
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

/// <summary>Provides a base class for queries that use a selector for data store access and a projector for materializing results.</summary>
/// <typeparam name="TSelection">The source <see cref="Type" /> of the query.</typeparam>
/// <typeparam name="TProjection">The projection <see cref="Type" /> of the query.</typeparam>
[PublicAPI]
public abstract class Query<TSelection, TProjection> : IQuery<TSelection, TProjection>
{
    /// <summary>Gets or sets the projector to execute when materializing the results.</summary>
    protected Func<IQueryable<TSelection>, IEnumerable<TProjection>> Projector { get; set; } = null!;

    /// <summary>Gets or sets the selector to execute when querying the data store.</summary>
    protected Func<IDataSource, IQueryable<TSelection>> Selector { get; set; } = null!;

    /// <inheritdoc cref="IQuery{T}.Execute" />
    public IEnumerable<TProjection> Execute(IDataSource dataSource)
    {
        return Projector(Selector(dataSource));
    }

    /// <summary>Supports building simple fluent API by internally doing the work necessary to alter the current <see cref="Selector" />.</summary>
    /// <param name="predicate">The predicate to apply to the current data request.</param>
    /// <returns>The current <see cref="Query{T}" /> with the given <paramref name="predicate" /> applied.</returns>
    protected Query<TSelection, TProjection> AddPredicate(Expression<Func<TSelection, bool>> predicate)
    {
        var currentSelector = Selector;
        Selector = dataSource => currentSelector(dataSource).Where(predicate);

        return this;
    }
}
