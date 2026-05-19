using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Roles.GetAll;

internal sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetAllRolesQuery, IReadOnlyList<RoleResponse>>
{
    public async Task<Result<IReadOnlyList<RoleResponse>>> Handle(
        GetAllRolesQuery query,
        CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(true, cancellationToken);

        var response = roles
            .OrderBy(role => role.Name)
            .Select(role => new RoleResponse(role.Id, role.Name, role.Description))
            .ToList();

        return Result.Success<IReadOnlyList<RoleResponse>>(response);
    }
}
