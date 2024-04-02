namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using TestSupport.Migrators;

public class MsSqlDatabaseDeployer : DatabaseDeployer
{
    private readonly string _connectionString;

    public MsSqlDatabaseDeployer(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override Assembly MigrationAssembly => GetType().Assembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        EnsureDatabase.For.SqlDatabase(_connectionString);
        var upgradeResult = DeployChanges.To.SqlDatabase(_connectionString).WithScriptsEmbeddedInAssembly(migrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
