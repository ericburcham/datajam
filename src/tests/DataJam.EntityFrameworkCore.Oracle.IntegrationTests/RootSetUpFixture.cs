namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests;

using System.Threading.Tasks;

using NUnit.Framework;

using Testcontainers.Oracle;

using TestSupport.Dependencies;
using TestSupport.Dependencies.TestContainers;
using TestSupport.Migrations;

[SetUpFixture]
internal class RootSetUpFixture() : TestContainerSetUpFixture<TestDependencyProvider>(TestDependencyProvider.Instance)
{
    public override async Task RunBeforeAllTests()
    {
        await base.RunBeforeAllTests();

        await DeployOracle();
    }

    private static async Task DeployOracle()
    {
        var oracleContainer = RegisteredTestDependencies.Get<OracleContainer>(ContainerConstants.ORACLE_CONTAINER_NAME);
        var connectionString = oracleContainer.GetConnectionString();
        var databaseDeployer = new OracleDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
