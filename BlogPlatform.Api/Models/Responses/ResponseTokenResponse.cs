namespace BlogPlatform.Api.Models.Responses;

public class ResponseTokenResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
}
