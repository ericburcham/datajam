namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests;

using System;
using System.Collections.Generic;

using AwesomeAssertions;

using global::Oracle.ManagedDataAccess.Client;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

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
            var connectionString = RegisteredTestDependencies.Get<OracleContainer>(ContainerConstants.ORACLE_CONTAINER_NAME).GetConnectionString();

            // Query to see what tables exist in the database
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT table_name FROM user_tables ORDER BY table_name", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("=== TABLES IN DATABASE ===");
                        while (reader.Read())
                        {
                            Console.WriteLine($"Table: {reader["table_name"]}");
                        }

                        Console.WriteLine("=== END TABLES ===");
                    }
                }
            }

            using (var db = new BloggingContext())
            {
                var blog = new Blog { BlogId = 1, Url = "https://blogs.oracle.com" };

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
            optionsBuilder.UseOracle(connectionString)
                         .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning))
                         .LogTo(Console.WriteLine, LogLevel.Information)
                         .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("BLOG");
                entity.HasKey(e => e.BlogId);
                entity.Property(e => e.BlogId)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();
                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("POST");
                entity.HasKey(e => e.PostId);
                entity.Property(e => e.PostId)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();
                entity.Property(e => e.BlogId)
                    .HasColumnName("BLOGID");
                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.BlogId);
            });
        }
    }
}
