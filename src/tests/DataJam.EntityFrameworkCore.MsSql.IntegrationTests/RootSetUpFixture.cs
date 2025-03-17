namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

using Testcontainers.MsSql;

using TestSupport.TestContainers;

[SetUpFixture]
internal class RootSetUpFixture() : TestContainerSetUpFixture<TestContainerProvider>(TestContainerProvider.Instance)
{
    public override async Task RunBeforeAllTests()
    {
        await base.RunBeforeAllTests();

        await DeployMsSql();
    }

    private static async Task DeployMsSql()
    {
        var sqlContainer = RegisteredContainers.Get<MsSqlContainer>(ContainerNames.SQL_SERVER);
        var connectionString = sqlContainer.GetConnectionString();
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "test-db" };
        connectionString = connectionStringBuilder.ConnectionString;
        var databaseDeployer = new MsSqlDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
