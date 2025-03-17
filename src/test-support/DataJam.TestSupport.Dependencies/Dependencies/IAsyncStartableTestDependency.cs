namespace DataJam.TestSupport.Dependencies;

using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Provides an interface for test dependencies that can be started and stopped asynchronously.</summary>
[PublicAPI]
public interface IAsyncStartableTestDependency : IStartableTestDependency
{
    /// <inheritdoc cref="IStartableTestDependency.Start" />
    void IStartableTestDependency.Start()
    {
        // Default implementation should just invoke the StartAsync method.
        StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="IStartableTestDependency.Stop" />
    void IStartableTestDependency.Stop()
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

/// <summary>Provides a strongly-typed interface for test dependencies that can be started and stopped asynchronously.</summary>
[PublicAPI]
public interface IAsyncStartableTestDependency<out T> : IAsyncStartableTestDependency, IStartableTestDependency<T>
    where T : class;
