namespace DataJam.EntityFramework.MsSql.IntegrationTests;

using System.Data.SqlClient;
using System.Threading.Tasks;

using NUnit.Framework;

using Testcontainers.MsSql;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;
using TestSupport.Migrations;

[SetUpFixture]
internal class RootSetUpFixture() : TestContainerSetUpFixture<TestDependencyProvider>(TestDependencyProvider.Instance)
{
    public override async Task RunBeforeAllTests()
    {
        await base.RunBeforeAllTests();

        await DeployMsSql();
    }

    private static async Task DeployMsSql()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        var sqlContainer = RegisteredTestDependencies.Get<MsSqlContainer>(ContainerConstants.MSSQL_CONTAINER_NAME);
        var connectionString = sqlContainer.GetConnectionString();
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = ContainerConstants.MSSQL_TEST_DB };
        connectionString = connectionStringBuilder.ConnectionString;
        var databaseDeployer = new MsSqlDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
