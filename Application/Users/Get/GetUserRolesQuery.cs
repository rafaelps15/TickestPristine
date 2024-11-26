using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;

namespace Tickest.Application.Users.Get;

public  class GetUserRolesQuery : IQuery<UserResponse>
{
    public Guid? Id { get; set; }
}




