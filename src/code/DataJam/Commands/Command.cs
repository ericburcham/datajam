namespace DataJam;

using System;

public class Command : ICommand
{
    protected Action<IDataContext> ContextCommand { get; set; } = null!;

    public void Execute(IDataContext dataContext)
    {
        ContextCommand(dataContext);
    }
}
