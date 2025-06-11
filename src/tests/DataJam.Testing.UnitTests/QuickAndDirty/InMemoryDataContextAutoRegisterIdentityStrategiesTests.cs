namespace DataJam.Testing.UnitTests.QuickAndDirty;

using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;

using NUnit.Framework;

[TestFixture]
public class InMemoryDataContextAutoRegisterIdentityStrategiesTests
{
    private DataContext _context = null!;

    [TestCase]
    public void Add_ShouldNotChangeIdWhenIdExisting()
    {
        // Arrange
        var entity = new Entity { Id = 25 };

        // Act
        _context.Add(entity);

        // Assert
        entity.Id.Should().Be(25);
    }

    [TestCase]
    public void Add_ShouldUseIdentityForRelatedCollectionTypes()
    {
        // Arrange
        var entity = new Entity();
        entity.MyProperties.Add(new());

        // Act
        _context.Add(entity);
        _context.Commit();

        // Assert
        entity.MyProperties.Single().Id.Should().NotBe(0);
    }

    [TestCase]
    public void Add_ShouldUseIdentityForRelatedTypes()
    {
        // Arrange
        var entity = new Entity { MyProperty = new() };

        // Act
        _context.Add(entity);
        _context.Commit();

        // Assert
        entity.MyProperty.Id.Should().NotBe(0);
    }

    [TestCase]
    public void Add_ShouldUseIdentityForType()
    {
        // Arrange
        var entity = new Entity();

        // Act
        _context.Add(entity);
        _context.Commit();

        // Assert
        entity.Id.Should().NotBe(0);
    }

    [TestCase]
    public void Commit_ShouldUseIdentityForRelatedCollectionTypes()
    {
        // Arrange
        var entity = new Entity();
        _context.Add(entity);
        _context.Commit();
        entity.MyProperties.Add(new());

        // Act
        _context.Commit();

        // Assert
        entity.MyProperties.Single().Id.Should().NotBe(0);
    }

    [SetUp]
    public void SetUp()
    {
        _context = new();
    }

    private class Entity : IIdentifiable<int>
    {
        public int Id { get; set; }

        public List<AnotherProperty> MyProperties { get; } = [];

        public AnotherProperty MyProperty { get; init; } = null!;
    }

    private class AnotherProperty : IIdentifiable<short>
    {
        public short Id { get; set; }
    }
}
