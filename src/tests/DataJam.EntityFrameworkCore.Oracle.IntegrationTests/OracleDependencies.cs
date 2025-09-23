namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests;

using System;

using global::Oracle.ManagedDataAccess.Client;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

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

            return new DbContextOptionsBuilder()
                  .UseOracle(connectionStringBuilder.ConnectionString)
                  .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning))
                  .LogTo(Console.WriteLine, LogLevel.Information)
                  .EnableSensitiveDataLogging()
                  .Options;
        }
    }
}
