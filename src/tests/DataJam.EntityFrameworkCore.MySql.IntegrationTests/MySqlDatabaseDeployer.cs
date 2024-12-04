namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using TestSupport;

public class MySqlDatabaseDeployer(string connectionString) : DatabaseDeployer
{
    protected override Assembly MigrationAssembly => GetType().Assembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        EnsureDatabase.For.MySqlDatabase(connectionString);
        var upgradeResult = DeployChanges.To.MySqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(migrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
