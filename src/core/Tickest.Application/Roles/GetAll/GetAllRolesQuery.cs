using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Roles.GetAll;

public sealed record GetAllRolesQuery : IQuery<IReadOnlyList<RoleResponse>>;
