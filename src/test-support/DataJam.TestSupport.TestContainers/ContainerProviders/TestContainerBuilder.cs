namespace DataJam.TestSupport.TestContainers;

using DotNet.Testcontainers.Containers;

public abstract class TestContainerBuilder<T> : IBuildTestContainers<T>
    where T : IContainer
{
    public abstract T Build();
}
