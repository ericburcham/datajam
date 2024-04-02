namespace DataJam.EntityFrameworkCore.IntegrationTests.Sqlite;

using System;

using Microsoft.EntityFrameworkCore;

using TestSupport.EntityFrameworkCore;

public class SqliteDependencies : Singleton<SqliteDependencies>, IProvideDbContextOptions
{
    private static readonly Lazy<DbContextOptions> _dbContextOptions = new(BuildDbContextOptions);

    private static readonly Lazy<SqliteMockContainer> _sqlite = new(BuildSqlite);

    public static SqliteMockContainer SqliteMockContainer => _sqlite.Value;

    public DbContextOptions Options => _dbContextOptions.Value;

    private static DbContextOptions BuildDbContextOptions()
    {
        return new DbContextOptionsBuilder().UseSqlite(SqliteMockContainer.GetConnectionString()).Options;
    }

    private static SqliteMockContainer BuildSqlite()
    {
        return new();
    }
}
