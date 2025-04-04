﻿namespace DataJam.Testing.UnitTests.QuickAndDirty;

using System;
using System.Collections.Generic;
using System.Linq;

using Domain;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class InMemoryDataContextTests
{
    private DataContext _context = null!;

    [TestCase]
    public void Add_ShouldCreateAFirstIdOfOneEveryTimeForANewInstanceOfIIdentifiableOfInt()
    {
        // Arrange
        var longPerson = new IdentifiablePerson<int>();
        _context.Add(longPerson);
        _context.Commit();

        // Act
        var newLongPerson = new IdentifiablePerson<int>();
        _context = new();
        _context.Add(newLongPerson);
        _context.Commit();

        // Assert
        newLongPerson.Id.Should().Be(1);
    }

    [TestCase]
    public void Add_ShouldCreateAFirstIdOfOneEveryTimeForANewInstanceOfIIdentifiableOfLong()
    {
        // Arrange
        var longPerson = new IdentifiablePerson<long>();
        _context.Add(longPerson);
        _context.Commit();

        // Act
        var newLongPerson = new IdentifiablePerson<long>();
        _context = new();
        _context.Add(newLongPerson);
        _context.Commit();

        // Assert
        newLongPerson.Id.Should().Be(1);
    }

    [TestCase]
    public void Add_ShouldCreateAFirstIdOfOneEveryTimeForANewInstanceOfIIdentifiableOfShort()
    {
        // Arrange
        var longPerson = new IdentifiablePerson<short>();
        _context.Add(longPerson);
        _context.Commit();

        // Act
        var newLongPerson = new IdentifiablePerson<short>();
        _context = new();
        _context.Add(newLongPerson);
        _context.Commit();

        // Assert
        newLongPerson.Id.Should().Be(1);
    }

    [TestCase]
    public void Add_ShouldIgnoreNonReferenceTypeProperties()
    {
        // arrange
        var site = new Site { Id = 2 };

        // act
        _context.Add(site);
        _context.Commit();

        // assert
        _context.Repo.Representations.Count(x => x.IsType<int>()).Should().Be(0);
    }

    [TestCase]
    public void Add_ShouldIgnoreNullCollections()
    {
        // arrange
        var blog = new Blog();

        // act
        _context.Add(blog);

        // assert
        _context.CreateQuery<Post>().Should().HaveCount(0);
    }

    [TestCase]
    public void Add_ShouldIncludeAllRelatedItemsFromRelatedCollections()
    {
        // arrange
        var blog = new Blog { Posts = new List<Post> { new(), new() } };

        // act
        _context.Add(blog);
        _context.Commit();

        // assert
        _context.CreateQuery<Post>().Should().HaveCount(2);
    }

    [TestCase]
    public void Add_ShouldNotAddAnItemTwiceForMultipleReferences()
    {
        // arrange
        var post = new Post();
        var blog = new Blog { Posts = new List<Post> { post, new() } };

        var blog2 = new Blog { Posts = new List<Post> { post } };

        _context.Add(blog);
        _context.Commit();

        // act
        _context.Add(blog2);
        _context.Commit();

        // assert
        _context.CreateQuery<Post>().Should().HaveCount(2);
    }

    [TestCase]
    public void Add_ShouldStoreASite()
    {
        // arrange
        var item = new Site();

        // act
        _context.Add(item);
        _context.Commit();

        // assert
        var site = _context.CreateQuery<Site>().First();
        site.Should().BeSameAs(item);
    }

    [TestCase]
    public void Add_ShouldStoreBlogWithAuthor()
    {
        // arrange
        var blog = new Blog { Author = new() };

        // act
        _context.Add(blog);
        _context.Commit();

        // assert
        _context.CreateQuery<Author>().Should().HaveCount(1);
        _context.CreateQuery<Author>().First().Should().BeSameAs(blog.Author);
    }

    [TestCase]
    public void Add_ShouldStoreThreeLevelsOfObjects()
    {
        // arrange
        var site = new Site { Blog = new() { Author = new() } };

        // act
        _context.Add(site);
        _context.Commit();

        // assert
        _context.CreateQuery<Author>().Should().HaveCount(1);
        _context.CreateQuery<Author>().First().Should().BeSameAs(site.Blog.Author);
    }

    [TestCase]
    public void Add_ShouldStoreTwoSites()
    {
        // arrange
        var item = new Site();
        var item2 = new Site();

        // act
        _context.Add(item);
        _context.Add(item2);
        _context.Commit();

        // assert
        _context.CreateQuery<Site>().Any(s => ReferenceEquals(item, s)).Should().BeTrue();

        _context.CreateQuery<Site>().Any(s => ReferenceEquals(item2, s)).Should().BeTrue();
    }

    [TestCase]
    public void Add_ShouldUseIdentityForRelatedCollectionTypes()
    {
        // Arrange
        _context.RegisterIdentityStrategy(new Int32IdentityStrategy<Post>(x => x.Id));
        var blog = new Blog();
        blog.Posts.Add(new() { Id = 0 });

        // Act
        _context.Add(blog);
        _context.Commit();

        // Assert
        blog.Posts.Single().Id.Should().NotBe(0);
    }

    [TestCase]
    public void Add_ShouldUseIdentityForRelatedTypes()
    {
        // Arrange
        _context.RegisterIdentityStrategy(new GuidIdentityStrategy<Author>(x => x.Id));
        var blog = new Blog { Author = new() { Id = Guid.Empty } };

        // Act
        _context.Add(blog);
        _context.Commit();

        // Assert
        blog.Author.Id.Should().NotBe(Guid.Empty);
    }

    [TestCase]
    public void Add_ShouldUseIdentityForType()
    {
        // Arrange
        _context.RegisterIdentityStrategy(new Int32IdentityStrategy<Post>(x => x.Id));
        var post = new Post { Id = 0 };

        // Act
        _context.Add(post);
        _context.Commit();

        // Assert
        post.Id.Should().NotBe(0);
    }

    [TestCase]
    public void Commit_ShouldAddNewLeafMembers()
    {
        // Arrange
        var blog = new Blog();
        _context.Add(blog);
        _context.Commit();

        // Act
        blog.Posts.Add(new());
        _context.Commit();

        // Assert
        _context.CreateQuery<Post>().Should().HaveCount(1);
    }

    [TestCase]
    public void Commit_ShouldRemoveOrphanedCollectionMembers()
    {
        // Arrange
        var post1 = new Post();
        var post2 = new Post();
        var blog = new Blog { Posts = new List<Post> { post1, post2 } };

        _context.Add(blog);
        _context.Commit();

        blog.Posts.Remove(post2);

        // Act
        _context.Commit();

        // Assert
        _context.CreateQuery<Post>().Should().Contain(post1);
        _context.CreateQuery<Post>().Should().NotContain(post2);
    }

    [TestCase]
    public void Commit_ShouldRemoveOrphanedCollectionMembersWhenWholeCollectionRemoved()
    {
        // Arrange
        var post1 = new Post();
        var post2 = new Post();
        var blog = new Blog { Posts = new List<Post> { post1, post2 } };

        _context.Add(blog);
        _context.Commit();
        blog.Posts = null!;

        // Act
        _context.Commit();

        // Assert
        _context.CreateQuery<Post>().Should().NotContain(post1);
        _context.CreateQuery<Post>().Should().NotContain(post2);
        _context.CreateQuery<Post>().Should().HaveCount(0);
    }

    [TestCase]
    public void Commit_ShouldRemoveOrphanedMembers()
    {
        // Arrange
        var blog = new Blog();
        var site = new Site { Blog = blog };
        _context.Add(site);
        _context.Commit();
        site.Blog = null!;

        // Act
        _context.Commit();

        // Assert
        _context.CreateQuery<Blog>().Should().NotContain(blog);
        _context.CreateQuery<Blog>().Should().HaveCount(0);
    }

    [TestCase]
    public void Commit_ShouldUseIdentityForRelatedCollectionTypes()
    {
        // Arrange
        _context.RegisterIdentityStrategy(new Int32IdentityStrategy<Post>(x => x.Id));
        var blog = new Blog();
        _context.Add(blog);
        _context.Commit();
        blog.Posts.Add(new() { Id = 0 });

        // Act
        _context.Commit();

        // Assert
        blog.Posts.Single().Id.Should().NotBe(0);
    }

    [TestCase]
    public void Remove_ShouldDeleteAnObjectFromRelatedObjects()
    {
        // arrange
        var site = new Site { Blog = new() };
        _context.Add(site);
        _context.Commit();

        // act
        _context.Remove(site.Blog);
        _context.Commit();

        // assert
        _context.CreateQuery<Blog>().Should().HaveCount(0);
        _context.CreateQuery<Site>().First().Blog.Should().BeNull();
    }

    [TestCase]
    public void Remove_ShouldNotRemoveIfReferencedByAnotherCollection()
    {
        // Arrange
        var post1 = new Post();
        var post2 = new Post();
        var blog1 = new Blog { Posts = new List<Post> { post1, post2 } };

        var blog2 = new Blog { Posts = new List<Post> { post1 } };

        _context.Add(blog1);
        _context.Add(blog2);
        _context.Commit();

        // Act
        _context.Remove(post2);
        _context.Commit();

        // Assert
        _context.CreateQuery<Post>().Should().HaveCount(1);
        _context.CreateQuery<Post>().First().Should().BeSameAs(post1);
        _context.CreateQuery<Blog>().Count(b => b.Posts.Count > 1).Should().Be(0);
    }

    [TestCase]
    public void Remove_ShouldNotRemoveIfReferencedByAnotherObject()
    {
        // Arrange
        var blog = new Blog();
        var site1 = new Site { Blog = blog };
        var site2 = new Site { Blog = blog };
        _context.Add(site1);
        _context.Add(site2);
        _context.Commit();

        // Act
        _context.Remove(site1);
        _context.Commit();

        // Assert
        _context.CreateQuery<Site>().Should().HaveCount(1);
        _context.CreateQuery<Site>().First().Should().BeSameAs(site2);
        _context.CreateQuery<Blog>().Should().HaveCount(1);
        _context.CreateQuery<Blog>().First().Should().BeSameAs(blog);
    }

    [TestCase]
    public void Remove_ShouldRemoveDependentGraphOnBranchRemoval()
    {
        // arrange
        var post = new Post();
        var blog = new Blog { Posts = new List<Post> { new(), post } };

        var site = new Site { Blog = blog };

        _context.Add(site);
        _context.Commit();

        // act
        _context.Remove(blog);
        _context.Commit();

        // assert
        var posts = _context.CreateQuery<Post>();
        posts.Should().HaveCount(0);
    }

    [TestCase]
    public void Remove_ShouldRemoveFromParentButNotDeleteChildObjectsThatAreReferencedMoreThanOne()
    {
        // arrange
        var post1 = new Post();
        var post2 = new Post();
        var blog1 = new Blog { Posts = new List<Post> { post1, post2 } };

        var blog2 = new Blog { Posts = new List<Post> { post1 } };

        var site = new Site { Blog = blog2 };

        _context.Add(blog1);
        _context.Add(site);
        _context.Commit();

        // Act
        _context.Remove(blog2);
        _context.Commit();

        // Assert
        _context.CreateQuery<Post>().Should().HaveCount(2);
        _context.CreateQuery<Post>().First().Should().BeSameAs(post1);
        _context.CreateQuery<Blog>().Single().Posts.Should().HaveCount(2);
        site.Blog.Should().BeNull();
    }

    [TestCase]
    public void Remove_ShouldRemoveObjectFromRelatedCollection()
    {
        // arrange
        var post = new Post();
        var blog = new Blog { Posts = new List<Post> { post } };
        _context.Add(blog);
        _context.Commit();

        // act
        _context.Remove(post);
        _context.Commit();

        // assert
        _context.CreateQuery<Blog>().Should().HaveCount(1);
        _context.CreateQuery<Blog>().First().Posts.Should().HaveCount(0);
        _context.CreateQuery<Post>().Should().HaveCount(0);
    }

    [SetUp]
    public void SetUp()
    {
        _context = new();
    }
}
