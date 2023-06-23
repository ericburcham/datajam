namespace DataJam.DataContexts;

using System;
using System.Linq;
using System.Threading.Tasks;

using Domain;

using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext, IDataContext
{
    private readonly IConfigureDomainMappings _mappingConfiguration;

    public DataContext(string connectionString, IConfigureDomainMappings mappingConfiguration)
        : base(GetOptions(connectionString))
    {
        _mappingConfiguration = mappingConfiguration;
    }

    public new T Add<T>(T item)
        where T : class
    {
        Set<T>().Add(item);

        return item;
    }

    public IQueryable<T> AsQueryable<T>()
        where T : class
    {
        return Set<T>();
    }

    public int Commit()
    {
        ChangeTracker.DetectChanges();

        return SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        ChangeTracker.DetectChanges();

        return await SaveChangesAsync();
    }

    public new T Remove<T>(T item)
        where T : class
    {
        return Set<T>().Remove(item).Entity;
    }

    public new T Update<T>(T item)
        where T : class
    {
        Entry(item).State = EntityState.Modified;

        return item;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _mappingConfiguration.ApplyDomainMappings(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static DbContextOptions GetOptions(string connectionString)
    {
        var builder = new DbContextOptionsBuilder();

        return builder.UseSqlServer(connectionString).Options;
    }
}

public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyDataContext
{
    public IQueryable<T> AsQueryable<T>()
        where T : class
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public class ReadonlyDbContext
{
}
