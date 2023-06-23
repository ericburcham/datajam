namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using TestSupport.Entities;

public abstract class Person : Entity
{
    public string Name { get; init; } = null!;
}
