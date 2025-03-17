namespace DataJam.TestSupport.Dependencies.TestContainers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using NUnit.Framework;

public abstract class TestDependencySetUpFixture<TDependencyProvider> : IAsyncDisposable
    where TDependencyProvider : CompositeTestDependencyProvider
{
    private readonly IEnumerable<object> _dependencies;

    protected TestDependencySetUpFixture(IEnumerable<TDependencyProvider> dependencyProviders)
        : this(dependencyProviders.ToArray())
    {
    }

    protected TestDependencySetUpFixture(params TDependencyProvider[] dependencyProviders)
    {
        _dependencies = dependencyProviders.SelectMany(x => x.TestDependencies);
    }

    public async ValueTask DisposeAsync()
    {
        await Parallel.ForEachAsync(
            _dependencies,
            async (dependency, _) =>
            {
                if (dependency is IAsyncDisposable disposable)
                {
                    await disposable.DisposeAsync();
                }
            });

        GC.SuppressFinalize(this);
    }

    [OneTimeTearDown]
    public virtual async Task RunAfterAllTests()
    {
        await Parallel.ForEachAsync(
            _dependencies,
            async (dependency, token) =>
            {
                if (dependency is IContainer container)
                {
                    await container.StopAsync(token);
                }
            });
    }

    [OneTimeSetUp]
    public virtual async Task RunBeforeAllTests()
    {
        await Parallel.ForEachAsync(
            _dependencies,
            async (dependency, token) =>
            {
                if (dependency is IContainer container)
                {
                    await container.StartAsync(token);
                }
            });
    }
}
