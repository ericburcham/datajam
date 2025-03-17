namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System;

using TestSupport.TestContainers;

internal class TestContainerProvider : CompositeContainerProvider
{
    private static readonly Lazy<TestContainerProvider> _instance = new(() => new());

    private TestContainerProvider()
    {
        Register(ContainerNames.SQL_SERVER, new DefaultMsSqlTestContainerBuilder());
    }

    public static TestContainerProvider Instance => _instance.Value;
}
