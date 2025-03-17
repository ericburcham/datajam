namespace DataJam.TestSupport.Dependencies.TestContainers;

using Testcontainers.MsSql;

public class DefaultMsSqlTestContainerBuilder : TestDependencyBuilder<MsSqlContainer>
{
    public override MsSqlContainer Build()
    {
        return new MsSqlBuilder().Build();
    }
}
