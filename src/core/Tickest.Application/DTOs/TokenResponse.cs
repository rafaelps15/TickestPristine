namespace Tickest.Application.DTOs;

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public string TokenType { get; set; } = "Bearer";
    public string Token { get; set; } = string.Empty;
}
