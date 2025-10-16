namespace BlogPlatform.Api.Models.Responses;

public class ResponseBlogPostDetailResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<ResponseCommentResponse> Comments { get; set; } = new();
}
