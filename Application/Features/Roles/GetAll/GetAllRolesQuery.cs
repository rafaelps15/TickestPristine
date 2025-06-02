using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Roles.GetAll;

public sealed record GetAllRolesQuery() : IQuery<List<RoleResponse>>;
