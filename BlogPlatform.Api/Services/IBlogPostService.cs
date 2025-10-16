using BlogPlatform.Api.Models;
using BlogPlatform.Api.Models.Requests;
using BlogPlatform.Api.Models.Responses;

namespace BlogPlatform.Api.Services;

public interface IBlogPostService
{
    Task<Result<List<ResponseBlogPostListResponse>>> ListAsync();
    Task<Result<ResponseBlogPostDetailResponse>> CreateAsync(BlogPostCreateRequest request);
    Task<Result<ResponseBlogPostDetailResponse>> GetDetailAsync(int id);
    Task<Result<ResponseCommentResponse>> AddCommentAsync(int postId, CommentCreateRequest request);
}
