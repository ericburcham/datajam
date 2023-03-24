namespace DataJam.Testing.UnitTests.QuickAndDirty.Domain;

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

    public Author Author { get; set; }

    public Guid Id { get; set; }

    public ICollection<Post> Posts { get; set; }

    public string Title { get; set; }
}
