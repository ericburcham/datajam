namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using System.Threading.Tasks;

[SetUpFixture]
public class RootSetUpFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await DeployMySql().ConfigureAwait(false);
    }

    private static async Task DeployMySql()
    {
        var connectionString = MySqlDependencies.Instance.MySql.GetConnectionString();
        var databaseDeployer = new MySqlDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
