namespace DataJam;

using System.Collections.Generic;
using System.Threading.Tasks;

public class Repository : IRepository
{
    private readonly IDataContext _dataContext;

    public Repository(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IUnitOfWork Context => _dataContext;

    public void Execute(ICommand command)
    {
        command.Execute(_dataContext);
    }

    public Task ExecuteAsync(ICommand command)
    {
        return Task.Factory.StartNew(() => command.Execute(_dataContext));
    }

    public IEnumerable<T> Find<T>(IQuery<T> query)
    {
        return query.Execute(_dataContext);
    }

    public T Find<T>(IScalar<T> scalar)
    {
        return scalar.Execute(_dataContext);
    }

    public IEnumerable<TProjection> Find<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
        where TSelection : class
    {
        return query.Execute(_dataContext);
    }

    public Task<T> FindAsync<T>(IScalar<T> scalar)
    {
        return Task.Factory.StartNew(() => scalar.Execute(_dataContext));
    }

    public Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
    {
        return Task.Factory.StartNew(() => query.Execute(_dataContext));
    }

    public Task<IEnumerable<TProjection>> FindAsync<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
        where TSelection : class
    {
        return Task.Factory.StartNew(() => query.Execute(_dataContext));
    }
}
