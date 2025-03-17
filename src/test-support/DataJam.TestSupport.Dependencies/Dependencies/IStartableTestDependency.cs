namespace DataJam.TestSupport.Dependencies;

using JetBrains.Annotations;

/// <summary>Provides an interface for test dependencies that can be started and stopped.</summary>
[PublicAPI]
public interface IStartableTestDependency : ITestDependency
{
    /// <summary>Starts the dependency.</summary>
    void Start();

    /// <summary>Stops the dependency.</summary>
    void Stop();
}

/// <summary>Provides a strongly-typed interface for test dependencies that can be started and stopped.</summary>
[PublicAPI]
public interface IStartableTestDependency<out T> : IStartableTestDependency, ITestDependency<T>
    where T : class;
