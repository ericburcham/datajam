namespace DataJam.TestSupport.TestContainers;

using DotNet.Testcontainers.Containers;

public static class RegisteredContainers
{
    public static T Get<T>(string name)
        where T : IContainer
    {
        return ContainerRegistry.Get<T>(name);
    }
}
