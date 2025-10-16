namespace BlogPlatform.Api.Models.Responses;

public class ResponseBlogPostListResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CommentCount { get; set; }
}
