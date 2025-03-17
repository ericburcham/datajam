namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using System;

using TestSupport.TestContainers;

public class TestContainerProvider : CompositeContainerProvider
{
    private static readonly Lazy<TestContainerProvider> _instance = new(() => new());

    private TestContainerProvider()
    {
        Register(ContainerConstants.MYSQL_CONTAINER_NAME, new DefaultMySqlTestContainerBuilder(ContainerConstants.MYSQL_USERNAME, ContainerConstants.MYSQL_PASSWORD));
    }

    public static TestContainerProvider Instance => _instance.Value;
}
