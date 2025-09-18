namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests;

using global::Oracle.ManagedDataAccess.Client;

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
            var connectionStringBuilder = new OracleConnectionStringBuilder(oracleContainer.GetConnectionString());

            return new DbContextOptionsBuilder().UseOracle(connectionStringBuilder.ConnectionString).Options;
        }
    }
}
