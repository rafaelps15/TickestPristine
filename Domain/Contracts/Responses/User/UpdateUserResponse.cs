namespace Tickest.Domain.Contracts.Responses.User;

public record UpdateUserResponse(Guid Id, string Email, string Name) : IApiResponse
{
    public string Message => throw new NotImplementedException();
}
