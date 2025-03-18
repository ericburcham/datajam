namespace DataJam.TestSupport.FluentMigrator.Core;

using System;
using System.Data;

using JetBrains.Annotations;

using Microsoft.Data.SqlClient;

[PublicAPI]
public static class SqlServerExtensions
{
    public static void SqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, int timeout = -1, string? collation = null)
    {
        GetMasterConnectionStringBuilder(connectionString, out var masterConnectionString, out var databaseName);

        using var connection = new SqlConnection(masterConnectionString);

        try
        {
            connection.Open();
        }
        catch (SqlException)
        {
            if (DatabaseExistsIfConnectedToDirectly(connectionString, databaseName))
            {
                return;
            }

            throw;
        }

        if (DatabaseExists(connection, databaseName))
        {
            return;
        }

        var collationString = string.IsNullOrEmpty(collation) ? string.Empty : $"COLLATE {collation}";
        var sqlCommandText = $"CREATE DATABASE [{databaseName}]{collationString};";

        using var command = new SqlCommand(sqlCommandText, connection);
        command.CommandType = CommandType.Text;

        if (timeout >= 0)
        {
            command.CommandTimeout = timeout;
        }

        command.ExecuteNonQuery();
    }

    private static bool DatabaseExists(SqlConnection connection, string databaseName)
    {
        var sqlCommandText = $"SELECT TOP 1 case WHEN dbid IS NOT NULL THEN 1 ELSE 0 end FROM sys.sysdatabases WHERE name = '{databaseName}';";

        // check to see if the database already exists..
        using var command = new SqlCommand(sqlCommandText, connection);

        command.CommandType = CommandType.Text;
        var results = Convert.ToInt32(command.ExecuteScalar());

        // if the database exists, we're done here...
        return results == 1;
    }

    private static bool DatabaseExistsIfConnectedToDirectly(string connectionString, string databaseName)
    {
        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            return DatabaseExists(connection, databaseName);
        }
        catch
        {
            return false;
        }
    }

    private static void GetMasterConnectionStringBuilder(string connectionString, out string masterConnectionString, out string databaseName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        var masterConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        databaseName = masterConnectionStringBuilder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException("The connection string does not specify a database name.");
        }

        masterConnectionStringBuilder.InitialCatalog = "master";
        masterConnectionString = masterConnectionStringBuilder.ConnectionString;
    }
}
