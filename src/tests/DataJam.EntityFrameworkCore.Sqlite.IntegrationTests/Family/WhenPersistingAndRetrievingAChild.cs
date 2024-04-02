namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests.Family;

[TestFixtureSource(typeof(TestFixtureConstructorParameterProvider), nameof(TestFixtureConstructorParameterProvider.Repositories))]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild
{
    public WhenPersistingAndRetrievingAChild(IRepository repository, bool useAmbientTransaction)
        : base(repository, useAmbientTransaction)
    {
    }
}
