namespace DataJam.Testing.UnitTests.QuickAndDirty;

using System.Linq;

using AwesomeAssertions;

using Domain;

using NUnit.Framework;

[TestFixture]
public class InMemoryBackReferencePopulation
{
    [TestCase]
    public void ShouldPopulateCollectionBasedReference()
    {
        var context = new DataContext();

        var child = new Post();
        var blog = new Blog("Test");
        child.Blog = blog;

        context.Add(child);
        context.Commit();

        blog.Posts.Count(x => x == child).Should().Be(1);
    }

    [TestCase]
    public void ShouldPopulateCollectionBasedReferenceReplacingNullCollection()
    {
        var context = new DataContext();

        var child = new Post();
        var blog = new Blog("Test");
        child.Blog = blog;
        blog.Posts = null!;

        context.Add(child);
        context.Commit();

        blog.Posts.Count(x => x == child).Should().Be(1);
    }

    [TestCase]
    public void ShouldPopulateSingleReference()
    {
        var context = new DataContext();

        var child = new Post();
        var blog = new Blog("Test");
        blog.Posts.Add(child);

        context.Add(blog);
        context.Commit();

        child.Blog.Should().NotBeNull();
    }
}
