namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using System.Collections.Generic;

public abstract class Parent : Person
{
    public ICollection<Child> Children { get; set; } = new List<Child>();
}