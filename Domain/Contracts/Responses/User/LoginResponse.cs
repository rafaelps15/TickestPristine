namespace Tickest.Domain.Contracts.Responses.User;

public record LoginResponse(Guid Id, string Email, string Name, TokenResponse TokenResponse) : IResponse;