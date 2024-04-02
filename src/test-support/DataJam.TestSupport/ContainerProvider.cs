namespace DataJam.TestSupport;

using System.Collections.Generic;

using DotNet.Testcontainers.Containers;

public class ContainerProvider : Singleton<ContainerProvider>, IProvideContainers, IRegisterContainers
{
    private readonly ICollection<IContainer> _containers = new List<IContainer>();

    public IEnumerable<IContainer> Containers => _containers;

    public void Register(IContainer container)
    {
        _containers.Add(container);
    }
}
