﻿namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

[TestFixtureSource(typeof(TestFixtureConstructorParameterProvider), nameof(TestFixtureConstructorParameterProvider.Repositories))]
public class WhenPersistingAndRetrievingAChild(IRepository repository) : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild(repository);
