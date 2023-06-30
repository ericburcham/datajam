namespace DataJam;

using System;

public class Scalar<TResult> : IScalar<TResult>
{
    protected Func<IDataSource, TResult> Selector { get; set; } = null!;

    public TResult Execute(IDataSource dataSource)
    {
        return Selector(dataSource);
    }
}
