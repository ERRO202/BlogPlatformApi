using BlogPlatform.Api.Data;
using BlogPlatform.Api.Models;
using BlogPlatform.Api.Models.Entities;
using BlogPlatform.Api.Models.Requests;
using BlogPlatform.Api.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatform.Api.Services;

public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepository _repo;

    public BlogPostService(IBlogPostRepository repo) => _repo = repo;

    public async Task<Result<List<ResponseBlogPostListResponse>>> ListAsync()
    {
        var posts = await _repo.Query().Include(p => p.Comments).ToListAsync();
        var list = posts.Select(p => new ResponseBlogPostListResponse
        {
            Id = p.Id,
            Title = p.Title,
            CommentCount = p.Comments?.Count ?? 0
        }).ToList();
        return Result<List<ResponseBlogPostListResponse>>.Ok(list);
    }

    public async Task<Result<ResponseBlogPostDetailResponse>> CreateAsync(BlogPostCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<ResponseBlogPostDetailResponse>.Fail("Title is required.");
        if (string.IsNullOrWhiteSpace(request.Content))
            return Result<ResponseBlogPostDetailResponse>.Fail("Content is required.");

        var entity = new BlogPost
        {
            Title = request.Title.Trim(),
            Content = request.Content.Trim(),
            Comments = new List<Comment>()
        };
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();

        return Result<ResponseBlogPostDetailResponse>.Ok(ToDetail(entity));
    }

    public async Task<Result<ResponseBlogPostDetailResponse>> GetDetailAsync(int id)
    {
        var post = await _repo.Query().Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
            return Result<ResponseBlogPostDetailResponse>.Fail($"Post {id} not found.");
        return Result<ResponseBlogPostDetailResponse>.Ok(ToDetail(post));
    }

    public async Task<Result<ResponseCommentResponse>> AddCommentAsync(int postId, CommentCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Author))
            return Result<ResponseCommentResponse>.Fail("Author is required.");
        if (string.IsNullOrWhiteSpace(request.Text))
            return Result<ResponseCommentResponse>.Fail("Text is required.");

        var post = await _repo.Query().Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null)
            return Result<ResponseCommentResponse>.Fail($"Post {postId} not found.");

        var comment = new Comment { Author = request.Author.Trim(), Text = request.Text.Trim() };
        post.Comments ??= new List<Comment>();
        post.Comments.Add(comment);
        await _repo.SaveChangesAsync();

        return Result<ResponseCommentResponse>.Ok(new ResponseCommentResponse
        {
            Id = comment.Id,
            Author = comment.Author,
            Text = comment.Text
        });
    }

    private static ResponseBlogPostDetailResponse ToDetail(BlogPost p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Content = p.Content,
        Comments = (p.Comments ?? new()).Select(c => new ResponseCommentResponse
        {
            Id = c.Id,
            Author = c.Author,
            Text = c.Text
        }).ToList()
    };
}
