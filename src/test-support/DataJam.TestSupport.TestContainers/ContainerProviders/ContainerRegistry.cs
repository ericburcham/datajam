namespace DataJam.TestSupport.TestContainers;

using System;
using System.Collections.Concurrent;

using DotNet.Testcontainers.Containers;

internal static class ContainerRegistry
{
    private static readonly ConcurrentDictionary<string, IContainer> _containers = new();

    public static void Add<T>(string name, T container)
        where T : IContainer
    {
        if (!_containers.TryAdd(name, container))
        {
            throw new ArgumentException($"A container with the name \"{name}\" already exists in the {nameof(ContainerRegistry)}.");
        }
    }

    public static T Get<T>(string name)
        where T : IContainer
    {
        return (T)_containers[name];
    }
}
