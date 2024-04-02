namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

[TestFixtureSource(typeof(TestFixtureConstructorParameterProvider), nameof(TestFixtureConstructorParameterProvider.Repositories))]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild
{
    public WhenPersistingAndRetrievingAChild(IRepository repository, bool useAmbientTransaction)
        : base(repository, useAmbientTransaction)
    {
    }
}
