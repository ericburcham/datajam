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

            // For Oracle, we need to make sure we're connecting to the same schema where tables were created
            // The default Oracle container usually uses 'system' user, but FluentMigrator creates tables in 'TESTSCHEMA'
            var connectionStringBuilder = new global::Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder(connectionString);

            // If the connection is using 'system' user, we need to switch to the test schema user
            if (connectionStringBuilder.UserID?.ToUpper() == "SYSTEM")
            {
                connectionStringBuilder.UserID = "TESTSCHEMA";
                connectionStringBuilder.Password = "password"; // This is set in OracleExtensions.cs
            }

            return new DbContextOptionsBuilder().UseOracle(connectionStringBuilder.ConnectionString).Options;
        }
    }
}
