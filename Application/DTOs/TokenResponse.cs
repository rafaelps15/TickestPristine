namespace Tickest.Application.DTOs;

/// <summary>
/// Representa a resposta contendo o token gerado e a data de expiração.
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// O token de acesso gerado.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// A data e hora de expiração do token.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// O tipo de token gerado (geralmente "Bearer" para tokens JWT).
    /// </summary>
    public string TokenType { get; set; } = "Bearer";
    public string Token { get; set; }
}
