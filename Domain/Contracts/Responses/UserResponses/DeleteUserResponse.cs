
namespace Tickest.Domain.Contracts.Responses.UserResponses;

public record DeleteUserResponse(int Id, string Email, string Name) : IResponse;
