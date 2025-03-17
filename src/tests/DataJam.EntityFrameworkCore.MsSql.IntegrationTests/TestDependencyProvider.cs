namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;

internal class TestDependencyProvider : CompositeTestDependencyProvider
{
    private static readonly Lazy<TestDependencyProvider> _instance = new(() => new());

    private TestDependencyProvider()
    {
        Register(ContainerConstants.MSSQL_CONTAINER_NAME, new DefaultMsSqlTestContainerBuilder());
    }

    public static TestDependencyProvider Instance => _instance.Value;
}
