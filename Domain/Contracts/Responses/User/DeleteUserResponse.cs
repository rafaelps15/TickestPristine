namespace Tickest.Domain.Contracts.Responses.User;

public record DeleteUserResponse(Guid Id, string Email, string Message) : IApiResponse;
