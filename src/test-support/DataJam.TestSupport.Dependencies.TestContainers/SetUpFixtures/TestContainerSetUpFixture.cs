namespace DataJam.TestSupport.Dependencies.TestContainers;

using System.Collections.Generic;

using JetBrains.Annotations;

[PublicAPI]
public abstract class TestContainerSetUpFixture<TDependencyProvider> : TestDependencySetUpFixture<TDependencyProvider>
    where TDependencyProvider : CompositeTestDependencyProvider
{
    protected TestContainerSetUpFixture(IEnumerable<TDependencyProvider> dependencyProviders)
        : base(dependencyProviders)
    {
    }

    protected TestContainerSetUpFixture(params TDependencyProvider[] dependencyProviders)
        : base(dependencyProviders)
    {
    }
}
