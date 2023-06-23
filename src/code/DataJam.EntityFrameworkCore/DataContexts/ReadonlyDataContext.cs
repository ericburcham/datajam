namespace DataJam.DataContexts;

using System;
using System.Linq;

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
