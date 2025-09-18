namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests;

using System;
using System.Collections.Generic;

using AwesomeAssertions;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using Testcontainers.Oracle;

using TestSupport.Dependencies;

[TestFixture]
public class QuickTests
{
    [Test]
    public void CanWorkWithOracle()
    {
        try
        {
            using (var db = new BloggingContext())
            {
                var blog = new Blog { Url = "https://blogs.oracle.com" };

                db.Blogs!.Add(blog);
                db.SaveChanges();
            }

            using (var db = new BloggingContext())
            {
                var blogs = db.Blogs;

                foreach (var item in blogs!)
                {
                    item.Url.Should().NotBeNull();
                }
            }
        }
        catch (DbUpdateException e)
        {
            Assert.Fail(e.Message);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }

        public List<Post>? Posts { get; set; }

        public string? Url { get; set; }
    }

    public class Post
    {
        public Blog? Blog { get; set; }

        public int BlogId { get; set; }

        public string? Content { get; set; }

        public int PostId { get; set; }

        public string? Title { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog>? Blogs { get; set; }

        public DbSet<Post>? Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = RegisteredTestDependencies.Get<OracleContainer>(ContainerConstants.ORACLE_CONTAINER_NAME).GetConnectionString();
            optionsBuilder.UseOracle(connectionString);
        }
    }
}
