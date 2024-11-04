namespace Tickest.Domain.Contracts.Responses.UserResponses;

public record LoginResponse(int Id, string Email, string Name, TokenResponse TokenResponse) : IResponse;