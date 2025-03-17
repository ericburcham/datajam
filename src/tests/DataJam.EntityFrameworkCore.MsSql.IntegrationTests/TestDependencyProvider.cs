namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System;
using System.Threading;
using System.Threading.Tasks;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;

internal class TestDependencyProvider : CompositeTestDependencyProvider
{
    private static readonly Lazy<TestDependencyProvider> _instance = new(() => new());

    private TestDependencyProvider()
    {
        Register(ContainerConstants.MSSQL_CONTAINER_NAME, new DefaultMsSqlTestContainerBuilder());
        Register(nameof(UselessTestDependency), new UselessTestDependencyBuilder());
    }

    public static TestDependencyProvider Instance => _instance.Value;
}

internal class UselessTestDependency : IStartableTestDependency, IDisposable
{
    public string Dependency => string.Empty;

    object ITestDependency.Dependency => Dependency;

    public void Dispose()
    {
        Console.WriteLine($"{nameof(UselessTestDependency)} disposed.");
    }

    public void Start()
    {
        Console.WriteLine($"{nameof(UselessTestDependency)} started.");
    }

    public void Stop()
    {
        Console.WriteLine($"{nameof(UselessTestDependency)} stopped.");
    }
}

internal class UselessTestDependencyBuilder : IBuildTestDependencies<UselessTestDependency>
{
    public UselessTestDependency Build()
    {
        return new();
    }
}
