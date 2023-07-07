namespace DataJam;

using System;

/// <summary>Provides a base class for commands that return no value, or where the value is not used.</summary>
public abstract class Command : ICommand
{
    /// <summary>Gets or sets the command action to execute.</summary>
    protected Action<IDataContext> ContextCommand { get; set; } = null!;

    /// <inheritdoc cref="ICommand" />
    public void Execute(IDataContext dataContext)
    {
        ContextCommand(dataContext);
    }
}
