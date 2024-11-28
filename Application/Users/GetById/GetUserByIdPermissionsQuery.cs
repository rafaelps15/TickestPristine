using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;

namespace Tickest.Application.Users.GetById;

public class GetUserByIdPermissionsQuery : IQuery<IEnumerable<Permission>>
{
    public Guid UserId { get; set; }
}


