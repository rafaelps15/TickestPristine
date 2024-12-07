using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;

namespace Tickest.Application.Users.GetById;

public class GetByIdUserQuery : IQuery<Result<UserResponse>>
{
    public Guid UserId { get; set; }

    public GetByIdUserQuery(Guid userId)
    {
        UserId = userId;
    }
}
