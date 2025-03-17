namespace DataJam.TestSupport.TestContainers;

using DotNet.Testcontainers.Containers;

public interface IBuildTestContainers<out T>
    where T : IContainer
{
    T Build();
}
