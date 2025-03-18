namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using System;

using TestSupport.Dependencies;
using TestSupport.Dependencies.Sqlite;

internal class TestDependencyProvider : CompositeTestDependencyProvider
{
    private static readonly Lazy<TestDependencyProvider> _instance = new(() => new());

    private TestDependencyProvider()
    {
        Register(DependencyConstants.SQLITE_DEPENDENCY_NAME, new SqliteTestDependencyBuilder());
    }

    public static TestDependencyProvider Instance => _instance.Value;
}
