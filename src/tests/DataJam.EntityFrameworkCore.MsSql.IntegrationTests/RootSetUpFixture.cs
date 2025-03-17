namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

using Testcontainers.MsSql;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;
using TestSupport.FluentMigrator.Deployers;

[SetUpFixture]
internal class RootSetUpFixture() : TestDependencySetUpFixture<TestDependencyProvider>(TestDependencyProvider.Instance)
{
    public override async Task RunBeforeAllTests()
    {
        await base.RunBeforeAllTests();

        await DeployMsSql();
    }

    private static async Task DeployMsSql()
    {
        var sqlContainer = RegisteredTestDependencies.Get<MsSqlContainer>(ContainerConstants.MSSQL_CONTAINER_NAME);
        var connectionString = sqlContainer.GetConnectionString();
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = ContainerConstants.MSSQL_TEST_DB };
        connectionString = connectionStringBuilder.ConnectionString;
        var databaseDeployer = new MsSqlDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
