namespace Tickest.Domain.Contracts.Responses.UserResponses;

public record CreateUserResponse(int Id, string Email, string Name) : IResponse;
