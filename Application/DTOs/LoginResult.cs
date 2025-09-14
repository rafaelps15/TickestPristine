namespace Tickest.Application.DTOs;

/// <summary>
/// Representa a resposta contendo o token gerado e a data de expiração.
/// </summary>
public record TokenResponse(
     Guid UserId,
     string Email,
     string AccessToken,
     DateTime ExpiresAt,
     string TokenType);


