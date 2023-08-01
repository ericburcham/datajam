namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

[TestFixtureSource(typeof(TestRepositoryProvider), nameof(TestRepositoryProvider.Repositories))]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.WhenPersistingAndRetrievingAChild
{
    public WhenPersistingAndRetrievingAChild(IRepository repository)
        : base(repository)
    {
    }
}
