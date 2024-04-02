namespace DataJam.EntityFrameworkCore.IntegrationTests.Sqlite;

using System;
using System.IO;

public class SqliteMockContainer
{
    private readonly string _connectionString;

    public SqliteMockContainer()
    {
        var guid = Guid.NewGuid();
        var executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var dataFile = Path.Combine(executableDirectory, $"{guid}.db");
        _connectionString = $"Data Source={dataFile}";
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }
}
