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
