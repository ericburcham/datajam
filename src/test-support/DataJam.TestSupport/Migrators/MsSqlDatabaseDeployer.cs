namespace DataJam.TestSupport.Migrators;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

public class MsSqlDatabaseDeployer : DatabaseDeployer
{
    private readonly string _connectionString;

    public MsSqlDatabaseDeployer(string connectionString, string migrationAssembly)
        : base(migrationAssembly)
    {
        _connectionString = connectionString;
    }

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
