namespace DataJam.Testing.UnitTests.QuickAndDirty.Domain;

public class Post
{
    public Blog Blog { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int Id { get; set; }

    public string Title { get; set; } = null!;
}
