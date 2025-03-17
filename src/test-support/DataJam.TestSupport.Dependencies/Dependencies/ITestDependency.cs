namespace DataJam.TestSupport.Dependencies;

using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Provides an interface for test dependencies.</summary>
[PublicAPI]
public interface ITestDependency
{
    object Dependency { get; }
}

/// <summary>Provides a marker interface for test dependencies.</summary>
[PublicAPI]
public interface ITestDependency<out T> : ITestDependency
{
    new T Dependency { get; }
}

/// <summary>Provides an interface for test dependencies that can be started and stopped.</summary>
[PublicAPI]
public interface IStartableTestDependency<out T> : ITestDependency<T>
{
    /// <summary>Starts the dependency.</summary>
    void Start();

    /// <summary>Stops the dependency.</summary>
    void Stop();
}

/// <summary>Provides an interface for test dependencies that can be started and stopped asynchronously.</summary>
[PublicAPI]
public interface IAsyncStartableTestDependency<out T> : IStartableTestDependency<T>
{
    /// <inheritdoc cref="IStartableTestDependency{T}.Start" />
    void IStartableTestDependency<T>.Start()
    {
        // Default implementation should just invoke the StartAsync method.
        StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="IStartableTestDependency{T}.Stop" />
    void IStartableTestDependency<T>.Stop()
    {
        // Default implementation should just invoke the StopAsync method.
        StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>Starts the dependency.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Task that completes when the dependency has been started.</returns>
    /// <exception cref="OperationCanceledException">Thrown when a call gets canceled.</exception>
    /// <exception cref="TaskCanceledException">Thrown when a task gets canceled.</exception>
    /// <exception cref="TimeoutException">Thrown when the wait strategy task gets canceled or the timeout expires.</exception>
    Task StartAsync(CancellationToken ct = default);

    /// <summary>Stops the dependency.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Task that completes when the dependency has been stopped.</returns>
    /// <exception cref="OperationCanceledException">Thrown when a call gets canceled.</exception>
    /// <exception cref="TaskCanceledException">Thrown when a task gets canceled.</exception>
    Task StopAsync(CancellationToken ct = default);
}
