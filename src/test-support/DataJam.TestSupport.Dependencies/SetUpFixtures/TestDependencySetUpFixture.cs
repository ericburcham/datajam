namespace DataJam.TestSupport.Dependencies;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using NUnit.Framework;

[PublicAPI]
public abstract class TestDependencySetUpFixture<T> : IAsyncDisposable, IDisposable
    where T : CompositeTestDependencyProvider
{
    private bool _disposed;

    protected TestDependencySetUpFixture(IEnumerable<T> dependencyProviders)
        : this(dependencyProviders.ToArray())
    {
    }

    protected TestDependencySetUpFixture(params T[] dependencyProviders)
    {
        Dependencies = dependencyProviders.SelectMany(x => x.TestDependencies);
    }

    ~TestDependencySetUpFixture()
    {
        Dispose(false);
    }

    protected IEnumerable<ITestDependency> Dependencies { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    [OneTimeTearDown]
    public virtual async Task RunAfterAllTests()
    {
        await Parallel.ForEachAsync(
            Dependencies,
            async (dependency, ct) =>
            {
                switch (dependency)
                {
                    case IAsyncStartableTestDependency startable:
                        await startable.StopAsync(ct);

                        break;

                    case IStartableTestDependency startable:
                        startable.Stop();

                        break;
                }
            });
    }

    [OneTimeSetUp]
    public virtual async Task RunBeforeAllTests()
    {
        await Parallel.ForEachAsync(
            Dependencies,
            async (dependency, ct) =>
            {
                switch (dependency)
                {
                    case IAsyncStartableTestDependency startable:
                        await startable.StartAsync(ct);

                        break;

                    case IStartableTestDependency startable:
                        startable.Start();

                        break;
                }
            });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (!disposing)
        {
            return;
        }

        // For dependencies that implement IAsyncDisposable, we need to call DisposeAsync
        // but since we're in a synchronous context, we'll need to wait for the task to complete
        foreach (var dependency in Dependencies)
        {
            switch (dependency)
            {
                case IAsyncDisposable disposable:
                    // Run the async operation synchronously
                    disposable.DisposeAsync().AsTask().GetAwaiter().GetResult();

                    break;

                case IDisposable disposable:
                    disposable.Dispose();

                    break;
            }
        }

        _disposed = true;
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_disposed)
        {
            return;
        }

        // First handle all IAsyncDisposable dependencies
        await Parallel.ForEachAsync(
            Dependencies,
            async (dependency, _) =>
            {
                switch (dependency)
                {
                    case IAsyncDisposable disposable:
                        await disposable.DisposeAsync();

                        break;

                    // Handle IDisposable dependencies that are not IAsyncDisposable
                    case IDisposable disposable:
                        disposable.Dispose();

                        break;
                }
            });

        _disposed = true;
    }
}
