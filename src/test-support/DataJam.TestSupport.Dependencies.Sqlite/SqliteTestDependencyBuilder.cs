namespace DataJam.TestSupport.Dependencies.Sqlite;

public class SqliteTestDependencyBuilder : TestDependencyBuilder<SqliteTestDependency>
{
    public override SqliteTestDependency Build()
    {
        return new();
    }
}
