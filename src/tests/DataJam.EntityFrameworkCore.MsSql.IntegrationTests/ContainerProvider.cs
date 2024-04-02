namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Collections.Generic;

using DotNet.Testcontainers.Containers;

using TestSupport;

public class ContainerProvider : Singleton<ContainerProvider>, IProvideContainers, IRegisterContainers
{
    private readonly ICollection<IContainer> _containers = new List<IContainer>();

    public IEnumerable<IContainer> Containers => _containers;

    public void Register(IContainer container)
    {
        _containers.Add(container);
    }
}
