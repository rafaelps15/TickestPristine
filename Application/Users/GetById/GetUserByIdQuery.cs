using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;

namespace Tickest.Application.Users.GetById;

public class GetUserByIdQuery : IQuery<UserResponse>
{
    public Guid Id { get; set; }
}
