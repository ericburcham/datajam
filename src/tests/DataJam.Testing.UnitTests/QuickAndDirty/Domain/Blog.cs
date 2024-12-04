namespace DataJam.Testing.UnitTests.QuickAndDirty.Domain;

using System;
using System.Collections.Generic;

public class Blog()
{
    public Blog(string title)
        : this()
    {
        Title = title;
    }

    public Author Author { get; set; } = null!;

    public Guid Id { get; set; } = Guid.NewGuid();

    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public string Title { get; set; } = null!;
}
