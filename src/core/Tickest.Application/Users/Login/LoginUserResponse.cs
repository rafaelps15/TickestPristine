namespace Tickest.Application.Users.Login;

public sealed record LoginUserResponse(
    string AccessToken,
    string TokenType,
    DateTime ExpiresAt
);
