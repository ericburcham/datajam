namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using TestSupport;

public class MsSqlDatabaseDeployer(string connectionString) : DatabaseDeployer
{
    protected override Assembly MigrationAssembly => GetType().Assembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);
        var upgradeResult = DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(migrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
