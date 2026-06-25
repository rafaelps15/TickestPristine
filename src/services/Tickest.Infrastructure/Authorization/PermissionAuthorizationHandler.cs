using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Infrastructure.Authentication;

namespace Tickest.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            context.Fail();

            return;
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPermissionProvider permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();

        Guid userId = context.User.GetUserId();

        HashSet<string> permissions = await permissionProvider.GetPermissionsForUserAsync(userId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);

            return;
        }
    }
}


