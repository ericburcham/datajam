namespace DataJam.TestSupport.Dependencies;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

[PublicAPI]
public abstract class TestDependencySetUpFixture<TDependencyProvider> : IAsyncDisposable, IDisposable
    where TDependencyProvider : CompositeTestDependencyProvider
{
    protected TestDependencySetUpFixture(IEnumerable<TDependencyProvider> dependencyProviders)
        : this(dependencyProviders.ToArray())
    {
    }

    protected TestDependencySetUpFixture(params TDependencyProvider[] dependencyProviders)
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

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();

        if (disposing)
        {
            Parallel.ForEach(
                Dependencies,
                (dependency, _) =>
                {
                    if (dependency is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                });
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await Parallel.ForEachAsync(
            Dependencies,
            async (dependency, _) =>
            {
                if (dependency is IAsyncDisposable disposable)
                {
                    await disposable.DisposeAsync();
                }
            });

        ReleaseUnmanagedResources();
    }

    private void ReleaseUnmanagedResources()
    {
        // Nothing to do here.
    }
}
