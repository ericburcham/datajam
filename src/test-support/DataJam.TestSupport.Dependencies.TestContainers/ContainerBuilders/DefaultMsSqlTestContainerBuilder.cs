namespace DataJam.TestSupport.Dependencies.TestContainers;

using Testcontainers.MsSql;

public class DefaultMsSqlTestContainerBuilder : ContainerBuilder<MsSqlContainer>
{
    protected override MsSqlContainer BuildContainer()
    {
        return new MsSqlBuilder().Build();
    }
}
