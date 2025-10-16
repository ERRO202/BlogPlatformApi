namespace BlogPlatform.Api.Models.Requests;

public class BlogPostCreateRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
