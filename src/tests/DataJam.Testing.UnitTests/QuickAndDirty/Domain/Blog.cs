namespace DataJam.Testing.UnitTests.QuickAndDirty.Domain;

using System;
using System.Collections.Generic;

public class Blog
{
    public Blog()
    {
        Posts = new List<Post>();
        Id = Guid.NewGuid();
    }

    public Blog(string title)
        : this()
    {
        Title = title;
    }

    public Author Author { get; set; } = null!;

    public Guid Id { get; set; }

    public ICollection<Post> Posts { get; set; }

    public string Title { get; set; } = null!;
}
