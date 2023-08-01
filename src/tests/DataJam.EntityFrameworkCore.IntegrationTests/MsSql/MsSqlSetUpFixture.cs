namespace DataJam.EntityFrameworkCore.IntegrationTests.MsSql;

using System.Threading.Tasks;

using TestSupport.Migrators;

[SetUpFixture]
public class MsSqlSetUpFixture
{
    private const string MIGRATION_ASSEMBLY = "DataJam.Migrations.MsSql";

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var connectionString = MsSqlDependencies.Instance.MsSql.GetConnectionString();
        var databaseDeployer = new MsSqlDatabaseDeployer(connectionString, MIGRATION_ASSEMBLY);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
