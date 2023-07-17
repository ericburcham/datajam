namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Collections.Generic;
using System.Linq;

using DotNet.Testcontainers.Containers;

using SqlServer;

using TestSupport;

public class ContainerProvider : Singleton<ContainerProvider>, IProvideContainers
{
    public IEnumerable<IContainer> Containers
    {
        get
        {
            return ContainerProviders.SelectMany(containerProvider => containerProvider.Containers);
        }
    }

    private static IEnumerable<IProvideContainers> ContainerProviders
    {
        get
        {
            yield return SqlServerDependencies.Instance;
        }
    }
}
