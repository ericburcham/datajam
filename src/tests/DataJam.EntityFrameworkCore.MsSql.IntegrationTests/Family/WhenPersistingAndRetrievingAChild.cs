namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

[TestFixtureSource(typeof(TestFixtureConstructorParameterProvider), nameof(TestFixtureConstructorParameterProvider.Repositories))]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild
{
    public WhenPersistingAndRetrievingAChild(IRepository repository)
        : base(repository)
    {
    }
}
