using System.Diagnostics;
using DataJam.Testing.UnitTests.QuickAndDirty.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.QuickAndDirty;

[TestFixture]
public class InMemoryDataContextPerfTests
{
    private TestDataContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new TestDataContext();
    }

    [TestCase]
    public void ShouldPerformBetterThan10MsInserts()
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < 100; i++)
        {
            _context.Add(
                new Site
                {
                    Blog = new Blog
                    {
                        Author = new Author(),
                        Id = Guid.NewGuid(),
                        Posts = new List<Post> { new Post(), new Post() }
                    }
                });
        }

        sw.Stop();
        var averageInsert = sw.ElapsedMilliseconds / 1000;
        averageInsert.Should().BeLessOrEqualTo(10);
        Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
    }

    [TestCase]
    public void ShouldPerformCommitsBetterThan10Ms()
    {
        //Arrange
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < 100; i++)
        {
            var blog = new Blog();
            _context.Add(blog);
            blog.Posts.Add(new Post());
            _context.Commit();
        }

        sw.Stop();
        var averageInsert = sw.ElapsedMilliseconds / 1000;
        averageInsert.Should().BeLessOrEqualTo(10);
        Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
    }
}