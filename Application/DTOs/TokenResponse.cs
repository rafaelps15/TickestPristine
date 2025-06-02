namespace Tickest.Application.DTOs;

/// <summary>
/// Representa a resposta contendo o token gerado e a data de expiração.
/// </summary>
public class TokenResponse
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
