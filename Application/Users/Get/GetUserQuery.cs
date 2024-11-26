using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;

namespace Tickest.Application.Users.Get;

public class GetUserQuery : IQuery<UserResponse>
{
    public Guid? Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
}
