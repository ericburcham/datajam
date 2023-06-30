namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Collections.Generic;

using DotNet.Testcontainers.Containers;

using Testcontainers.MsSql;

using TestSupport;

public class Dependencies : Singleton<Dependencies>, IProvideContainers
{
    private Dependencies()
    {
        MsSql = new MsSqlBuilder().Build();
    }

    public IEnumerable<IContainer> Containers
    {
        get
        {
            yield return MsSql;
        }
    }

    public MsSqlContainer MsSql { get; set; }
}
