namespace DataJam.TestSupport.TestContainers;

using System;
using System.Collections.Generic;

using DotNet.Testcontainers.Containers;

public abstract class CompositeContainerProvider : IProvideTestContainers
{
    private readonly Dictionary<string, IContainer> _containers = new();

    public IEnumerable<IContainer> TestContainers => _containers.Values;

    protected void Register<T>(string name, IBuildTestContainers<T> builder)
        where T : IContainer
    {
        var container = builder.Build();

        if (!_containers.TryAdd(name, container))
        {
            throw new ArgumentException($"A container with the name \'{name}\' already exists in the {GetType().Name}.");
        }

        ContainerRegistry.Add(name, container);
    }
}
