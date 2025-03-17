namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using System;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;

public class TestDependencyProvider : CompositeTestDependencyProvider
{
    private static readonly Lazy<TestDependencyProvider> _instance = new(() => new());

    private TestDependencyProvider()
    {
        Register(ContainerConstants.MYSQL_CONTAINER_NAME, new DefaultMySqlTestContainerBuilder(ContainerConstants.MYSQL_USERNAME, ContainerConstants.MYSQL_PASSWORD));
    }

    public static TestDependencyProvider Instance => _instance.Value;
}
