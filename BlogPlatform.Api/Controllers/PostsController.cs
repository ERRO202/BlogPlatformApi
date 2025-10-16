using BlogPlatform.Api.Models;
using BlogPlatform.Api.Models.Requests;
using BlogPlatform.Api.Models.Responses;
using BlogPlatform.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IBlogPostService _service;

    public PostsController(IBlogPostService service) => _service = service;

    // Public endpoints
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<Result<List<ResponseBlogPostListResponse>>>> GetAll()
        => Ok(await _service.ListAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Result<ResponseBlogPostDetailResponse>>> GetById([FromRoute] int id)
        => Ok(await _service.GetDetailAsync(id));

    // Protected endpoints
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Result<ResponseBlogPostDetailResponse>>> Create([FromBody] BlogPostCreateRequest request)
        => Ok(await _service.CreateAsync(request));

    [HttpPost("{id:int}/comments")]
    [Authorize]
    public async Task<ActionResult<Result<ResponseCommentResponse>>> AddComment([FromRoute] int id, [FromBody] CommentCreateRequest request)
        => Ok(await _service.AddCommentAsync(id, request));
}
