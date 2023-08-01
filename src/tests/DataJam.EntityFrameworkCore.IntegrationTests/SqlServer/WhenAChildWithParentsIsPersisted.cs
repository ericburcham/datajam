namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

[TestFixtureSource(typeof(TestRepositoryProvider), nameof(TestRepositoryProvider.Repositories))]
public class WhenAChildWithParentsIsPersisted : TestSupport.TestPatterns.WhenAChildWithParentsIsPersisted
{
    public WhenAChildWithParentsIsPersisted(IRepository repository)
        : base(repository)
    {
    }
}
