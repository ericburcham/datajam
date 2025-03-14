namespace DataJam.TestSupport.FluentMigrator;

public static class EnsureDatabase
{
    /// <summary>Gets the databases supported by the EnsureDatabase feature.</summary>
    public static SupportedDatabasesForEnsureDatabase For { get; } = new();
}
