namespace DataJam.TestSupport.TestPatterns.Domains.Family;

using System.Collections.Generic;

public abstract class Parent : Person
{
    public ICollection<Child> Children { get; } = new List<Child>();
}
