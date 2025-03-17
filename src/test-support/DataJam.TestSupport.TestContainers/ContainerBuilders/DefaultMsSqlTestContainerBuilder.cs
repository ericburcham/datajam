namespace DataJam.TestSupport.TestContainers;

using Testcontainers.MsSql;

public class DefaultMsSqlTestContainerBuilder : TestContainerBuilder<MsSqlContainer>
{
    public override MsSqlContainer Build()
    {
        return new MsSqlBuilder().Build();
    }
}
