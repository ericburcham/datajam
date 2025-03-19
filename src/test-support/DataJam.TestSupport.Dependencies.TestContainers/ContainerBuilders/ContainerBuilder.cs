namespace DataJam.TestSupport.Dependencies.TestContainers;

using DotNet.Testcontainers.Containers;

using JetBrains.Annotations;

[PublicAPI]
public abstract class ContainerBuilder<T> : TestDependencyBuilder<ContainerAdapter<T>>
    where T : class, IContainer
{
    public override ContainerAdapter<T> Build()
    {
        var container = BuildContainer();

        return new(container);
    }

    protected abstract T BuildContainer();
}
