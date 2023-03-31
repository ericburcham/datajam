namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using System;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsGuidWithIdentity : SingleEntityScenario<Guid>
{
    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(Guid id)
    {
        id.Should().NotBe(Guid.Empty);
    }
}
