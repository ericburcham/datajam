namespace DataJam.TestSupport.Dependencies;

using JetBrains.Annotations;

/// <summary>Provides an interface for test dependencies.</summary>
[PublicAPI]
public interface ITestDependency
{
    /// <summary>Gets the dependency.</summary>
    object Dependency { get; }
}

/// <summary>Provides a strongly-typed interface for test dependencies.</summary>
[PublicAPI]
public interface ITestDependency<out T> : ITestDependency
    where T : class
{
    /// <inheritdoc cref="ITestDependency.Dependency" />
    new T Dependency { get; }
}
