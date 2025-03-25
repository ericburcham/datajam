namespace DataJam;

using System;

using JetBrains.Annotations;

/// <summary>Provides a base class for commands that return no value, or where the value is not used.</summary>
[PublicAPI]
public abstract class Command : ICommand
{
    /// <summary>Gets the command action to execute.</summary>
    protected Action<IUnitOfWork> ContextCommand { get; } = null!;

    /// <inheritdoc cref="ICommand.Execute" />
    public void Execute(IUnitOfWork unitOfWork)
    {
        ContextCommand(unitOfWork);
    }
}
