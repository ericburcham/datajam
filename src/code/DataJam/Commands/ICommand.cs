namespace DataJam;

/// <summary>Exposes an interface for commands that return no value, or where the value is not used.</summary>
public interface ICommand
{
    /// <summary>Executes the command using the provided data context.</summary>
    /// <param name="dataContext">The data context to execute the command with.</param>
    void Execute(IDataContext dataContext);
}
