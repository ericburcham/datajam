namespace DataJam;

using System;

/// <summary>Provides a base class for scalar queries..</summary>
/// <typeparam name="TResult">The type of the expected result.</typeparam>
public abstract class Scalar<TResult> : IScalar<TResult>
{
    /// <summary>Gets or sets the scalar query to execute.</summary>
    protected Func<IDataSource, TResult> Selector { get; set; } = null!;

    /// <inheritdoc cref="IScalar{TResult}" />
    public TResult Execute(IDataSource dataSource)
    {
        return Selector(dataSource);
    }
}
