namespace BlogPlatform.Api.Models.Responses;

public class ResponseCommentResponse
{
    public int Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
