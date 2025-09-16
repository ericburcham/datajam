// Copyright (c) PlaceholderCompany. All rights reserved.

namespace DataJam.TestSupport.TestPatterns.Family;

using System.Linq;
using System.Threading.Tasks;

using AwesomeAssertions;

using EntityFrameworkCore;

using NUnit.Framework;

public abstract class WhenPersistingAndRetrievingAChild : TransactionalScenario
{
    private Child _result = null!;

    protected abstract IRepository Repository { get; }

    [Test]
    public void ItShouldHaveAValidId()
    {
        this._result.Id.Should().NotBe(0);
    }

    [Test]
    public void ItShouldHaveTheCorrectFather()
    {
        this._result.Father.Name.Should().Be("Dad");
    }

    [Test]
    public void ItShouldHaveTheCorrectMother()
    {
        this._result.Mother.Name.Should().Be("Mom");
    }

    [Test]
    public void ItShouldHaveTheCorrectName()
    {
        this._result.Name.Should().Be("Kid");
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Arrange
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        this.Repository.Context.Add(child);
        await this.Repository.Context.CommitAsync().ConfigureAwait(false);

        // Act
        var scalar = new GetChildren();
        this._result = this.Repository.Find(scalar).Single();
    }

    [OneTimeTearDown]
    protected override void OneTimeTearDown()
    {
        base.OneTimeTearDown();
        var dataContext = this.Repository.Context;
        dataContext.Dispose();
    }
}
