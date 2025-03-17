namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using System.Threading.Tasks;

using TestSupport.Dependencies;
using TestSupport.Dependencies.Sqlite;
using TestSupport.FluentMigrations.Sqlite.Deployers;

[SetUpFixture]
internal class RootSetUpFixture : TestDependencySetUpFixture<TestDependencyProvider>
{
    public RootSetUpFixture()
        : base(TestDependencyProvider.Instance)
    {
    }

    public override async Task RunBeforeAllTests()
    {
        await base.RunBeforeAllTests();

        await DeploySqlite();
    }

    private static async Task DeploySqlite()
    {
        var sqliteDb = RegisteredTestDependencies.Get<SqliteTestDependency>(DependencyConstants.SQLITE_DEPENDENCY_NAME);
        var connectionString = sqliteDb.GetConnectionString();
        var databaseDeployer = new SqliteDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
