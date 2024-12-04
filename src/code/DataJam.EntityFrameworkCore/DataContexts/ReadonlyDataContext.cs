namespace DataJam.EntityFrameworkCore;

using System;
using System.Linq;

/// <summary>Provides a data context which is limited to read operations.</summary>
public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyDataContext
{
    /// <inheritdoc cref="IDataSource.AsQueryable{TResult}" />
    public IQueryable<T> AsQueryable<T>()
        where T : class
    {
        throw new NotImplementedException();
    }
}
