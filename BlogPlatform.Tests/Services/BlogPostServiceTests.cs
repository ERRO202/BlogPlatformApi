using Xunit;
using BlogPlatform.Api.Data;
using BlogPlatform.Api.Models.Entities;
using BlogPlatform.Api.Models.Requests;
using BlogPlatform.Api.Services;
using BlogPlatform.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatform.Tests.Services;

public class BlogPostServiceTests
{
    private static (IBlogPostService service, AppDbContext db) BuildSutWithSeed()
    {
        var db = TestDb.CreateContext();
        db.BlogPosts.Add(new BlogPost
        {
            Title = "Hello World",
            Content = "Seed",
            Comments = new List<Comment> { new() { Author = "A", Text = "hi" } }
        });
        db.SaveChanges();

        var repo = new BlogPostRepository(db);
        var service = new BlogPostService(repo);
        return (service, db);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnPostsWithCommentCount()
    {
        var (service, _) = BuildSutWithSeed();

        var result = await service.ListAsync();

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data!.Count);
        Assert.Equal(1, result.Data![0].CommentCount);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenTitleMissing()
    {
        var db = TestDb.CreateContext();
        var repo = new BlogPostRepository(db);
        var service = new BlogPostService(repo);

        var result = await service.CreateAsync(new BlogPostCreateRequest { Title = "", Content = "x" });

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("Title is required"));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreate_WhenValid()
    {
        var db = TestDb.CreateContext();
        var repo = new BlogPostRepository(db);
        var service = new BlogPostService(repo);

        var result = await service.CreateAsync(new BlogPostCreateRequest { Title = "Post 1", Content = "Body" });

        Assert.True(result.Success);
        Assert.Equal("Post 1", result.Data!.Title);
    }

    [Fact]
    public async Task GetDetailAsync_ShouldFail_WhenNotFound()
    {
        var db = TestDb.CreateContext();
        var repo = new BlogPostRepository(db);
        var service = new BlogPostService(repo);

        var result = await service.GetDetailAsync(123);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("not found"));
    }

    [Fact]
    public async Task AddCommentAsync_ShouldAddComment()
    {
        var db = TestDb.CreateContext();
        var repo = new BlogPostRepository(db);
        var service = new BlogPostService(repo);

        var created = await service.CreateAsync(new BlogPostCreateRequest { Title = "t", Content = "c" });
        var id = created.Data!.Id;

        var add = await service.AddCommentAsync(id, new CommentCreateRequest { Author = "Juan", Text = "Nice" });

        Assert.Equal(1, await db.Comments.CountAsync());

        var detail = await service.GetDetailAsync(id);
        Assert.Equal(1, detail.Data!.Comments.Count);
        Assert.Equal("Nice", detail.Data!.Comments[0].Text);
    }

    [Fact]
    public async Task AddCommentAsync_ShouldFail_WhenPostMissing()
    {
        var db = TestDb.CreateContext();
        var repo = new BlogPostRepository(db);
        var service = new BlogPostService(repo);

        var result = await service.AddCommentAsync(999, new CommentCreateRequest { Author = "A", Text = "B" });

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("not found"));
    }
}
