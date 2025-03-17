namespace DataJam.TestSupport.Dependencies.TestContainers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using NUnit.Framework;

public abstract class TestContainerSetUpFixture<TDependencyProvider> : TestDependencySetUpFixture<TDependencyProvider>
    where TDependencyProvider : CompositeTestDependencyProvider
{
    private readonly IEnumerable<ITestDependency> _dependencies;

    protected TestContainerSetUpFixture(IEnumerable<TDependencyProvider> dependencyProviders)
        : this(dependencyProviders.ToArray())
    {
    }

    protected TestContainerSetUpFixture(params TDependencyProvider[] dependencyProviders)
    {
        _dependencies = dependencyProviders.SelectMany(x => x.TestDependencies);
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
