﻿namespace DataJam.TestSupport.TestPatterns.Family;

using System.Linq;
using System.Threading.Tasks;

using EntityFrameworkCore;

using FluentAssertions;

using NUnit.Framework;

public abstract class WhenPersistingAndRetrievingAChild : TransactionalScenario
{
    private readonly IRepository _repository;

    private Child _result = null!;

    protected WhenPersistingAndRetrievingAChild(IRepository repository)
    {
        _repository = repository;
    }

    private int StateEntriesWritten { get; set; }

    [Test]
    public void ItShouldHaveAValidId()
    {
        _result.Id.Should().NotBe(0);
    }

    [Test]
    public void ItShouldHaveTheCorrectFather()
    {
        _result.Father.Name.Should().Be("Dad");
    }

    [Test]
    public void ItShouldHaveTheCorrectMother()
    {
        _result.Mother.Name.Should().Be("Mom");
    }

    [Test]
    public void ItShouldHaveTheCorrectName()
    {
        _result.Name.Should().Be("Kid");
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Arrange
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        _repository.Context.Add(child);
        StateEntriesWritten = await _repository.Context.CommitAsync().ConfigureAwait(false);

        // Act
        var scalar = new GetChildren();
        _result = _repository.Find(scalar).Single();
    }

    [Test]
    public void TheRowCountShouldBeCorrect()
    {
        StateEntriesWritten.Should().Be(3);
    }

    [OneTimeTearDown]
    protected override void OneTimeTearDown()
    {
        base.OneTimeTearDown();
        var dataContext = (IDataContext)_repository.Context;
        dataContext.Dispose();
    }
}
