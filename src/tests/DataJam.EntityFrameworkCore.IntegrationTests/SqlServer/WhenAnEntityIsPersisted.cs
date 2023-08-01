namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

[TestFixtureSource(typeof(TestRepositoryProvider), nameof(TestRepositoryProvider.Services))]
public class WhenAnEntityIsPersisted : TestSupport.TestPatterns.WhenAnEntityIsPersisted
{
    public WhenAnEntityIsPersisted(IRepository repository)
        : base(repository)
    {
    }
}
