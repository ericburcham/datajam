namespace DataJam.TestSupport.Dependencies.TestContainers;

using DotNet.Testcontainers.Containers;

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
