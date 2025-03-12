namespace DataJam;

using System;

using JetBrains.Annotations;

/// <summary>Provides a base class for scalar queries..</summary>
/// <typeparam name="T">The type of the expected result.</typeparam>
[PublicAPI]
public abstract class Scalar<T> : IScalar<T>
{
    /// <summary>Gets or sets the scalar query to execute.</summary>
    protected Func<IDataSource, T> Selector { get; set; } = null!;

    /// <inheritdoc cref="IScalar{T}" />
    public T Execute(IDataSource dataSource)
    {
        return Selector(dataSource);
    }
}
