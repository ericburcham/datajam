namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests;

using System;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;

internal class TestDependencyProvider : CompositeTestDependencyProvider
{
    private static readonly Lazy<TestDependencyProvider> _instance = new(() => new());

    private TestDependencyProvider()
    {
        Register(ContainerConstants.ORACLE_CONTAINER_NAME, new DefaultOracleTestContainerBuilder(ContainerConstants.ORACLE_PASSWORD));
    }

    public static TestDependencyProvider Instance => _instance.Value;
}
