namespace DataJam.TestSupport.Dependencies;

using JetBrains.Annotations;

/// <summary>Provides an interface for test dependencies.</summary>
[PublicAPI]
public interface ITestDependency
{
    /// <summary>Gets the dependency.</summary>
    object Dependency { get; }
}
