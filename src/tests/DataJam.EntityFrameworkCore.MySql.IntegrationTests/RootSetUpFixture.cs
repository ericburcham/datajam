namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using System.Threading.Tasks;

using global::MySql.Data.MySqlClient;

using Testcontainers.MySql;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;
using TestSupport.FluentMigrator.Deployers;

[SetUpFixture]
public class RootSetUpFixture() : TestContainerSetUpFixture<TestDependencyProvider>(TestDependencyProvider.Instance)
{
    public override async Task RunBeforeAllTests()
    {
        await base.RunBeforeAllTests();

        await DeployMySql();
    }

    private static async Task DeployMySql()
    {
        var mySqlContainer = RegisteredTestDependencies.Get<MySqlContainer>(ContainerConstants.MYSQL_CONTAINER_NAME);
        var connectionString = mySqlContainer.GetConnectionString();
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString) { Database = ContainerConstants.MYSQL_TEST_DB };
        connectionString = connectionStringBuilder.ConnectionString;
        var databaseDeployer = new MySqlDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
