﻿namespace DataJam.Testing.UnitTests.QuickAndDirty;

using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

[TestFixture]
public class InMemoryDataContextReferenceByIdTests
{
    private DataContext _context = null!;

    [SetUp]
    public void SetUp()
    {
        _context = new();
    }

    [TestCase]
    public void ShouldBeAbleToIterate()
    {
        _context.Add(new Blog());
        _context.Add(new Blog());
        _context.Commit();

        try
        {
            foreach (var blogId in _context.CreateQuery<Blog>().Select(b => b.Id))
            {
                var post = new Post { BlogId = blogId };
                _context.Add(post);
            }
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    private class Blog : IIdentifiable<long>
    {
        public long Id { get; set; }

        public List<Post> Posts { get; } = [];
    }

    private class Post : IIdentifiable<long>
    {
        public long BlogId { get; set; }

        public long Id { get; set; }
    }
}
