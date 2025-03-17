namespace DataJam.TestSupport.Dependencies.TestContainers;

using System.Collections.Generic;

using JetBrains.Annotations;

[PublicAPI]
public abstract class TestContainerSetUpFixture<T> : TestDependencySetUpFixture<T>
    where T : CompositeTestDependencyProvider
{
    protected TestContainerSetUpFixture(IEnumerable<T> dependencyProviders)
        : base(dependencyProviders)
    {
    }

    protected TestContainerSetUpFixture(params T[] dependencyProviders)
        : base(dependencyProviders)
    {
    }
}
