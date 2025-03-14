// -----------------------------------------------------------------------
// <copyright file="MySqlExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DataJam.TestSupport.FluentMigrator;

using System;
using System.Data;

using MySql.Data.MySqlClient;

public static class MySqlExtensions
{
    public static void MySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, int timeout = -1, string? collation = null)
    {
        GetMysqlConnectionStringBuilder(connectionString, out var masterConnectionString, out var databaseName);

        try
        {
            using var connection = new MySqlConnection(masterConnectionString);
            connection.Open();

            if (DatabaseExists(connection, databaseName))
            {
                return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Database not found on server with connection string in settings: {e.Message}");
        }

        using (var connection = new MySqlConnection(masterConnectionString))
        {
            connection.Open();

            if (DatabaseExists(connection, databaseName))
            {
                return;
            }

            var collationString = string.IsNullOrEmpty(collation) ? string.Empty : $" COLLATE {collation}";

            var sqlCommandText = $"CREATE DATABASE {databaseName}{collationString};";

            // Create the database...
            using (var command = new MySqlCommand(sqlCommandText, connection) { CommandType = CommandType.Text })
            {
                if (timeout >= 0)
                {
                    command.CommandTimeout = timeout;
                }

                command.ExecuteNonQuery();
            }
        }
    }

    private static bool DatabaseExists(MySqlConnection connection, string databaseName)
    {
        var sqlCommandText = string.Format($"SELECT SCHEMA_NAME FROM information_schema.schemata WHERE SCHEMA_NAME = '{databaseName}';");

        // check to see if the database already exists..
        using (var command = new MySqlCommand(sqlCommandText, connection) { CommandType = CommandType.Text })
        {
            var result = command.ExecuteScalar();

            return result != null;
        }
    }

    private static void GetMysqlConnectionStringBuilder(string connectionString, out string masterConnectionString, out string databaseName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        var masterConnectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        databaseName = masterConnectionStringBuilder.Database;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException("The connection string does not specify a database name.");
        }

        masterConnectionStringBuilder.Database = "mysql";
        masterConnectionString = masterConnectionStringBuilder.ConnectionString;
    }
}
