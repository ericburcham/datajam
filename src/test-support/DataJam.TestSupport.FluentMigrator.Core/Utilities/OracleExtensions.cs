namespace DataJam.TestSupport.FluentMigrator.Core;

using System;
using System.Data;

using JetBrains.Annotations;

using Oracle.ManagedDataAccess.Client;

[PublicAPI]
public static class OracleExtensions
{
    public static void OracleDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, int timeout = -1)
    {
        GetMasterConnectionStringBuilder(connectionString, out var masterConnectionString, out var serviceName);

        using var connection = new OracleConnection(masterConnectionString);

        try
        {
            connection.Open();
        }
        catch (OracleException)
        {
            if (DatabaseExistsIfConnectedToDirectly(connectionString, serviceName))
            {
                return;
            }

            throw;
        }

        if (DatabaseExists(connection, serviceName))
        {
            return;
        }

        // For Oracle containers, we typically need to create a user/schema rather than a database
        var sqlCommandText = $"CREATE USER {serviceName} IDENTIFIED BY password DEFAULT TABLESPACE USERS TEMPORARY TABLESPACE TEMP";

        using var command = new OracleCommand(sqlCommandText, connection);
        command.CommandType = CommandType.Text;

        if (timeout >= 0)
        {
            command.CommandTimeout = timeout;
        }

        try
        {
            command.ExecuteNonQuery();

            // Grant necessary privileges
            var grantCommand = new OracleCommand($"GRANT CONNECT, RESOURCE, CREATE SESSION TO {serviceName}", connection);
            grantCommand.ExecuteNonQuery();
        }
        catch (OracleException ex) when (ex.Number == 1920)
        {
            // User already exists, this is acceptable
        }
    }

    private static bool DatabaseExists(OracleConnection connection, string serviceName)
    {
        var sqlCommandText = $"SELECT COUNT(*) FROM ALL_USERS WHERE USERNAME = UPPER('{serviceName}')";

        using var command = new OracleCommand(sqlCommandText, connection);
        command.CommandType = CommandType.Text;
        var results = Convert.ToInt32(command.ExecuteScalar());

        return results > 0;
    }

    private static bool DatabaseExistsIfConnectedToDirectly(string connectionString, string serviceName)
    {
        try
        {
            using var connection = new OracleConnection(connectionString);
            connection.Open();

            return DatabaseExists(connection, serviceName);
        }
        catch
        {
            return false;
        }
    }

    private static void GetMasterConnectionStringBuilder(string connectionString, out string masterConnectionString, out string serviceName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        var masterConnectionStringBuilder = new OracleConnectionStringBuilder(connectionString);
        serviceName = masterConnectionStringBuilder.UserID ?? "TESTSCHEMA";

        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new InvalidOperationException("The connection string does not specify a user ID.");
        }

        // Connect as system user to create schemas - Oracle containers typically have sys/Oracle
        masterConnectionStringBuilder.UserID = "sys";
        masterConnectionStringBuilder.Password = "Oracle";
        masterConnectionStringBuilder.DBAPrivilege = "SYSDBA";
        masterConnectionString = masterConnectionStringBuilder.ConnectionString;
    }
}
