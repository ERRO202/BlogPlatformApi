namespace BlogPlatform.Api.Models.Requests;

public class CommentCreateRequest
{
    public string Author { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
