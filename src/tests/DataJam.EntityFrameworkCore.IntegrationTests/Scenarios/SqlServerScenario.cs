namespace DataJam.EntityFrameworkCore.IntegrationTests;

using Microsoft.EntityFrameworkCore;

using SqlServer;

using TestSupport.EntityFrameworkCore;

public class SqlServerScenario : EntityFrameworkCoreScenario
{
    protected static readonly string CONNECTION_STRING = SqlServerDependencies.Instance.MsSql.GetConnectionString();

    protected override DbContextOptions DbContextOptions => new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options;
}
