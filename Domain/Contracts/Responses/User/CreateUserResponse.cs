namespace Tickest.Domain.Contracts.Responses.User;

public record CreateUserResponse(Guid Id, string Email, string Name) : IResponse { }

