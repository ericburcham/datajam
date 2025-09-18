namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

using Testcontainers.Oracle;

using TestSupport.Dependencies;

[UsedImplicitly]
public class OracleDependencies
{
    public static DbContextOptions Options
    {
        get
        {
            var oracleContainer = RegisteredTestDependencies.Get<OracleContainer>(ContainerConstants.ORACLE_CONTAINER_NAME);
            var connectionString = oracleContainer.GetConnectionString();

            return new DbContextOptionsBuilder().UseOracle(connectionString).Options;
        }
    }
}
