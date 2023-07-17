namespace DataJam.TestSupport.Domains.Family;

using DataJam.TestSupport.Entities;

public abstract class Person : Entity
{
    public string Name { get; init; } = null!;
}
