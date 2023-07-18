namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

using System.Threading.Tasks;

[SetUpFixture]
public class SetUpFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await SqlServerDependencies.Instance.DeployDatabase();
    }
}
