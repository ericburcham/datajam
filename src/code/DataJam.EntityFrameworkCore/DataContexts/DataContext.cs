namespace DataJam.DataContexts;

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext, IDataContext
{
    public new T Add<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }

    public IQueryable<TResult> AsQueryable<TResult>()
        where TResult : class
    {
        throw new NotImplementedException();
    }

    public int Commit()
    {
        throw new NotImplementedException();
    }

    public Task<int> CommitAsync()
    {
        throw new NotImplementedException();
    }

    public new T Remove<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }

    public new T Update<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }
}
