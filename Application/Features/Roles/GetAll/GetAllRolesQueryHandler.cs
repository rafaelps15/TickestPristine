using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Roles.GetAll;

internal sealed class GetAllRolesQueryHandler(
    IRoleRepository roleRepository
) : IQueryHandler<GetAllRolesQuery, List<RoleResponse>>
{
    public async Task<Result<List<RoleResponse>>> Handle(GetAllRolesQuery query, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(cancellationToken);

        var rolesDto = roles.Select(r => new RoleResponse(r.Id, r.Name)).ToList();

        return Result.Success(rolesDto);
    }
}
