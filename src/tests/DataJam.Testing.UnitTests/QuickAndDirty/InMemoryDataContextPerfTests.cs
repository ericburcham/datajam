namespace DataJam.Testing.UnitTests.QuickAndDirty;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Domain;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class InMemoryDataContextPerfTests
{
    private DataContext _context = null!;

    [SetUp]
    public void Setup()
    {
        _context = new();
    }

    [TestCase]
    public void ShouldPerformBetterThan10MsInserts()
    {
        var sw = Stopwatch.StartNew();

        for (var i = 0; i < 100; i++)
        {
            _context.Add(new Site { Blog = new() { Author = new(), Id = Guid.NewGuid(), Posts = new List<Post> { new(), new() } } });
        }

        sw.Stop();
        var averageInsert = sw.ElapsedMilliseconds / 1000;
        averageInsert.Should().BeLessOrEqualTo(10);
        Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
    }

    [TestCase]
    public void ShouldPerformCommitsBetterThan10Ms()
    {
        // Arrange
        var sw = Stopwatch.StartNew();

        for (var i = 0; i < 100; i++)
        {
            var blog = new Blog();
            _context.Add(blog);
            blog.Posts.Add(new());
            _context.Commit();
        }

        sw.Stop();
        var averageInsert = sw.ElapsedMilliseconds / 1000;
        averageInsert.Should().BeLessOrEqualTo(10);
        Console.WriteLine("Average Time for insert of graph is {0}", averageInsert);
    }
}
