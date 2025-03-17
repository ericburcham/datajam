// -----------------------------------------------------------------------
// <copyright file="SqliteTestDependency.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DataJam.TestSupport.Dependencies.Sqlite;

using System;
using System.IO;

using Microsoft.Data.Sqlite;

/// <summary>A test dependency that manages a SQLite database file.</summary>
public class SqliteTestDependency : IStartableTestDependency, IDisposable
{
    private readonly string _connectionString;

    private readonly string _databasePath;

    public SqliteTestDependency()
    {
        // Create a unique database file path
        var guid = Guid.NewGuid();
        var executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _databasePath = Path.Combine(executableDirectory, $"{guid}.db");
        _connectionString = $"Data Source={_databasePath}";
        Dependency = this;
    }

    public object Dependency { get; }

    public void Dispose()
    {
        // Clear connection pools and delete the database file
        SqliteConnection.ClearAllPools();

        if (File.Exists(_databasePath))
        {
            File.Delete(_databasePath);
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>Gets the connection string for the SQLite database.</summary>
    public string GetConnectionString()
    {
        return _connectionString;
    }

    public void Start()
    {
        // SQLite will create the database file on first connection
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
    }

    public void Stop()
    {
        // Clear connection pools to ensure we can delete the file during cleanup
        SqliteConnection.ClearAllPools();
    }
}

public class SqliteTestDependencyBuilder : TestDependencyBuilder<SqliteTestDependency>
{
    public override SqliteTestDependency Build()
    {
        return new();
    }
}
