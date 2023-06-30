namespace DataJam;

using System;
using System.Collections.Generic;
using System.Linq;

public class Query<TResult> : IQuery<TResult>
{
    protected Func<IDataSource, IQueryable<TResult>> Selector { get; set; } = null!;

    public IEnumerable<TResult> Execute(IDataSource dataSource)
    {
        return Selector(dataSource);
    }
}
