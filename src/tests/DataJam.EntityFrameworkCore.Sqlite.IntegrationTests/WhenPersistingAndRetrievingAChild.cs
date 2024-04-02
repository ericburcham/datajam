﻿namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

[TestFixtureSource(typeof(TestFixtureConstructorParameterProvider), nameof(TestFixtureConstructorParameterProvider.Repositories))]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.WhenPersistingAndRetrievingAChild
{
    public WhenPersistingAndRetrievingAChild(IRepository repository, bool useAmbientTransaction)
        : base(repository, useAmbientTransaction)
    {
    }
}
