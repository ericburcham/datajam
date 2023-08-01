namespace DataJam.EntityFrameworkCore.IntegrationTests.Sqlite;

using System;
using System.IO;
using System.Threading;

using Microsoft.EntityFrameworkCore;

using TestSupport.EntityFrameworkCore;

public class SqliteDependencies : Singleton<SqliteDependencies>, IProvideDbContextOptions
{
    private readonly ReaderWriterLockSlim _containerLock = new();

    public DbContextOptions Options => new DbContextOptionsBuilder().UseSqlite(Sqlite.GetConnectionString()).Options;

    public Sqlite Sqlite { get; } = new();
}

public class Sqlite
{
    private readonly string _connectionString;

    public Sqlite()
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
