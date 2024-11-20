using Tickest.Domain.Common;
using System;

namespace Tickest.Domain.Contracts.Responses;

public record TokenResponse : IResponse
{
    public TokenResponse(string accessToken)
    {
        AccessToken = ValidarAccessToken(accessToken);
    }

    public TokenResponse(string accessToken, string refreshToken) : this(accessToken)
    {
        RefreshToken = ValidarRefreshToken(refreshToken);
    }

    public TokenResponse(string accessToken, string refreshToken, DateTime expiresAt) : this(accessToken, refreshToken)
    {
        ExpiresAt = ValidarDataExpiracao(expiresAt);
    }

    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public DateTime ExpiresAt { get; init; }

    // Valida o access token
    private static string ValidarAccessToken(string accessToken) =>
        string.IsNullOrWhiteSpace(accessToken)
            ? throw new ArgumentNullException(nameof(accessToken), "O token de acesso não pode ser nulo ou vazio.")
            : accessToken;

    // Valida o refresh token
    private static string ValidarRefreshToken(string refreshToken) =>
        string.IsNullOrWhiteSpace(refreshToken)
            ? throw new ArgumentNullException(nameof(refreshToken), "O token de atualização não pode ser nulo ou vazio.")
            : refreshToken;

    // Valida a data de expiração
    private static DateTime ValidarDataExpiracao(DateTime expiresAt)
    {
        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentOutOfRangeException(nameof(expiresAt), "A data de expiração deve ser no futuro.");

        return expiresAt;
    }
}
