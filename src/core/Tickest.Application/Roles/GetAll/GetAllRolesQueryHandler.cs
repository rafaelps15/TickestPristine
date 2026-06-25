using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.Domain.Constants;
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
            .Where(r => r.IsActive && !r.IsDeleted && r.Name != SystemRoles.AdminMaster)
            .OrderBy(r => r.Name)
            .Select(r => new RoleResponse(r.Id, r.Name, r.Description))
            .ToList();

        return Result.Success<IReadOnlyList<RoleResponse>>(response);
    }
}
