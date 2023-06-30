namespace DataJam;

using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext, IDataContext
{
    private readonly IConfigureDomainMappings<ModelBuilder>? _mappingConfiguration;

    public DataContext(DbContextOptions options, IConfigureDomainMappings<ModelBuilder> mappingConfiguration)
        : base(options)
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
        _mappingConfiguration?.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
