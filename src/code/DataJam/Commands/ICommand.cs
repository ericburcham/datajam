namespace DataJam;

using JetBrains.Annotations;

/// <summary>Exposes an interface for commands that return no value, or where the value is not used.</summary>
[PublicAPI]
public interface ICommand
{
    /// <summary>Executes the command using the provided unit of work.</summary>
    /// <param name="unitOfWork">The unit of work to execute the command with.</param>
    void Execute(IUnitOfWork unitOfWork);
}
