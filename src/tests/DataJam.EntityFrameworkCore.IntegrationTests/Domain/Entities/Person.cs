namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using DataJam.TestSupport.Entities;

public abstract class Person : Entity
{
    public string Name { get; set; }
}