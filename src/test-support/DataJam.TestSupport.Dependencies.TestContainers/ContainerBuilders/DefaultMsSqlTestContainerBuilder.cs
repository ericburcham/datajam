namespace DataJam.TestSupport.Dependencies.TestContainers;

using JetBrains.Annotations;

using Testcontainers.MsSql;

[PublicAPI]
public class DefaultMsSqlTestContainerBuilder : ContainerBuilder<MsSqlContainer>
{
    protected override MsSqlContainer BuildContainer()
    {
        return new MsSqlBuilder().Build();
    }
}
