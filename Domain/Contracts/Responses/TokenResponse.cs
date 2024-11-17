using Tickest.Domain.Common;

namespace Tickest.Domain.Contracts.Responses;

public record TokenResponse : IResponse
{
    public TokenResponse(string accessToken)
    {
        AccessToken = accessToken;
    }

    public TokenResponse(string accessToken, DateTime expiresAt)
    {
        AccessToken = accessToken;
        ExpiresAt = expiresAt;
    }

    public string AccessToken { get; }
    public DateTime ExpiresAt { get; }
}

