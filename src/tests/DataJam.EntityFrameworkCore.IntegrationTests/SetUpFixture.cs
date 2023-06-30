namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

[SetUpFixture]
public class SetUpFixture
{
    private static Assembly MigrationAssembly => Assembly.Load("DataJam.Migrations");

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await StartContainers();
        await DeployMsSqlDatabase();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await StopContainers();
    }

    private Task DeployMsSqlDatabase()
    {
        var connectionString = Dependencies.Instance.MsSql.GetConnectionString();

        EnsureDatabase.For.SqlDatabase(connectionString);
        var upgradeResult = DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(MigrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }

    private async Task StartContainers()
    {
        await Parallel.ForEachAsync(
            Dependencies.Instance.Containers,
            async (container, token) =>
            {
                await container.StartAsync(token);
            });
    }

    private async Task StopContainers()
    {
        await Parallel.ForEachAsync(
            Dependencies.Instance.Containers,
            async (container, token) =>
            {
                await container.StopAsync(token);
            });
    }
}
