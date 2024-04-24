namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Threading.Tasks;

[SetUpFixture]
public class RootSetUpFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await DeployMsSql().ConfigureAwait(false);
    }

    private static async Task DeployMsSql()
    {
        var connectionString = MsSqlDependencies.Instance.MsSql.GetConnectionString();
        var databaseDeployer = new MsSqlDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
