namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using TestSupport.Migrators;

public class MySqlDatabaseDeployer : DatabaseDeployer
{
    private readonly string _connectionString;

    public MySqlDatabaseDeployer(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override Assembly MigrationAssembly => GetType().Assembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        EnsureDatabase.For.MySqlDatabase(_connectionString);
        var upgradeResult = DeployChanges.To.MySqlDatabase(_connectionString).WithScriptsEmbeddedInAssembly(migrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
