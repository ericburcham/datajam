namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using Microsoft.EntityFrameworkCore;

[SetUpFixture]
public class SetUpFixture
{
    private static Assembly MigrationAssembly => Assembly.Load("DataJam.Migrations");

    private string _connectionString;

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

    private async Task InsertFamilies()
    {
        var domain = new FamilyDomain(_connectionString);
        var dataContext = new DomainContext<FamilyDomain>(domain);

        foreach (var child in BuildFamilies())
        {
            dataContext.Add(child);
        }

        await dataContext.CommitAsync();
    }

    private Task DeployMsSqlDatabase()
    {
        var connectionString = Dependencies.Instance.MsSql.GetConnectionString();

        EnsureDatabase.For.SqlDatabase(connectionString);
        var upgradeResult = DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(MigrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            _connectionString = connectionString;
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }

    private async Task StartContainers()
    {
        foreach (var container in Dependencies.Instance.Containers)
        {
            await container.StartAsync();
        }
    }

    private async Task StopContainers()
    {
        foreach (var container in Dependencies.Instance.Containers)
        {
            await container.StopAsync();
        }
    }
}


public class MyContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}