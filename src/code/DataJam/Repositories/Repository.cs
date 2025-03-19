namespace DataJam;

using System.Collections.Generic;
using System.Threading.Tasks;

using DataContexts;

using JetBrains.Annotations;

/// <summary>Provides an implementation of the Repository pattern.</summary>
/// <param name="dataContext">The data context to use.</param>
[PublicAPI]
public class Repository(IDataContext dataContext) : IRepository
{
    /// <inheritdoc cref="IRepository.Context" />
    public IDataContext Context { get; } = dataContext;

    /// <inheritdoc cref="IRepository.Execute" />
    public void Execute(ICommand command)
    {
        command.Execute(Context);
    }

    /// <inheritdoc cref="IRepository.ExecuteAsync" />
    public Task ExecuteAsync(ICommand command)
    {
        return Task.Factory.StartNew(() => command.Execute(Context));
    }

    /// <inheritdoc cref="IRepository.Find{T}(IQuery{T})" />
    public IEnumerable<T> Find<T>(IQuery<T> query)
    {
        return query.Execute(Context);
    }

    /// <inheritdoc cref="IRepository.Find{T}(IScalar{T})" />
    public T Find<T>(IScalar<T> scalar)
    {
        return scalar.Execute(Context);
    }

    /// <inheritdoc cref="IRepository.FindAsync{T}(IQuery{T})" />
    public Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
    {
        return Task.Factory.StartNew(() => query.Execute(Context));
    }

    /// <inheritdoc cref="IRepository.FindAsync{T}(IScalar{T})" />
    public Task<T> FindAsync<T>(IScalar<T> scalar)
    {
        return Task.Factory.StartNew(() => scalar.Execute(Context));
    }
}
