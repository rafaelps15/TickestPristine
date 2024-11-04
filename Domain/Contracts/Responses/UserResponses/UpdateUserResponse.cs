namespace Tickest.Domain.Contracts.Responses.UserResponses;

public record UpdateUserResponse(int Id, string Email, string Name) : IResponse;