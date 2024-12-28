using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Application.Permissions.GetById;

public class GetByIdUserPermissionsQuery : IQuery<IEnumerable<Permission>>
{
    public Guid UserId { get; set; }
}


